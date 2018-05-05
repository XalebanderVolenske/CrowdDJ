using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using CrowdDj.DAL;
using CrowdDj.WPF.WindowControllers;

namespace CrowdDj.WPF.ViewModels
{
    public class LoginViewModel : BaseViewModel, INotifyDataErrorInfo, IValidatableObject
    {
        private string _textPartyCode;
        // Validation
        private bool _hasErrors;
        private bool _isValid;
        protected readonly Dictionary<string, List<string>> Errors; // Manage errors of properties
        private string _dbError;

        public string TextPartyCode
        {
            get { return _textPartyCode; }
            set
            {
                _textPartyCode = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private IWindowController _windowController;

        public Party SelectedParty { get; set; }

        public LoginViewModel() : base(null)
        {
        }

        public LoginViewModel(IWindowController contr, Party selectedParty) : base(contr)
        {
            _windowController = contr;
            SelectedParty = selectedParty;
            Errors = new Dictionary<string, List<string>>();
            LoadCommands();
        }

        public bool IsChanged
        {
            get { return TextPartyCode != ""; }
        }

        public RelayCommand CommandLogin { get; set; }
        public RelayCommand CommandBack { get; set; }

        /// <summary>
        /// Create commands with callbacks CanExecute and Undo
        /// </summary>
        private void LoadCommands()
        {
            CommandLogin = new RelayCommand(Login, c => true);
            CommandBack = new RelayCommand(Back, c => IsChanged);
        }

        private void Login(object obj)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            { 
                try
                {
                    int partyCode = Convert.ToInt32(TextPartyCode);
                    var partyGuest = unitOfWork.PartyGuests.Get(includeProperties:"Party")
                                           .SingleOrDefault(pg => pg.PartyCode == partyCode);
                    if (partyGuest != null && partyGuest.Party.Id.Equals(SelectedParty.Id))
                    {
                        Controller.CloseWindow();
                        new MainWindowViewModel(_windowController, partyGuest);
                    }
                }
                catch (ValidationException ex)
                {
                    string propertyName = (ex.Value as List<String>)[0];
                    Errors.Add(propertyName, new List<string> { ex.Message });
                    HasErrors = Errors.Any();
                    IsValid = Errors.Count == 0;
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                    return;
                }
                Controller.CloseWindow();
            }
            Controller.CloseWindow();
        }

        private void Back(object obj)
        {
            Controller.CloseWindow();
        }

        #region Validation

        // Validation in Wrapper-Basisklasse auslagern!

        /// <summary>
        /// Aufruf der Validierung aller Properties.
        /// </summary>
        private void Validate()
        {
            ClearErrors(); // alte Fehlermeldungen löschen
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this);
            Validator.TryValidateObject(this, validationContext, validationResults, true);
            if (validationResults.Any())
            {
                var propertyNames = validationResults.SelectMany(
                    validationResult => validationResult.MemberNames).Distinct().ToList();
                foreach (var propertyName in propertyNames)
                {
                    Errors[propertyName] = validationResults
                        .Where(validationResult => validationResult.MemberNames.Contains(propertyName))
                        .Select(r => r.ErrorMessage)
                        .Distinct() // gleiche Fehlermeldungen unterdrücken
                        .ToList();
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }

            HasErrors = Errors.Any();
            IsValid = Errors.Count == 0 && string.IsNullOrEmpty(DbError);
        }

        /// <summary>
        /// Fehlerliste löschen und Properties verständigen
        /// </summary>
        protected void ClearErrors()
        {
            DbError = "";
            foreach (var propertyName in Errors.Keys.ToList())
            {
                Errors.Remove(propertyName);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Fehlermeldungen für das Property zrückgeben
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>Fehlermeldungen für das Property</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return propertyName != null && Errors.ContainsKey(propertyName)
                ? Errors[propertyName]
                : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gibt es derzeit im ViewModel Fehler
        /// Damit die Notification funktioniert, wird umständlich ein 
        /// echtes Property angelegt.
        /// </summary>
        public bool HasErrors
        {
            get { return _hasErrors; }
            set
            {
                _hasErrors = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sind alle Properties valide, gibt es in der Errorscollection keine Einträge
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        public String DbError
        {
            get { return _dbError; }
            set
            {
                _dbError = value;
                OnPropertyChanged();
            }
        }



        /// <summary>
        /// Response to UI if errors have changed
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc />
        /// <summary>
        /// Validation over IValidatableObject
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            OnPropertyChanged(nameof(IsChanged));
            if (string.IsNullOrWhiteSpace(TextPartyCode))
            {
                yield return new ValidationResult("Partycode is required",
                    new[] {nameof(TextPartyCode)});
            }

            if (TextPartyCode != null && TextPartyCode.Length > 20)
            {
                yield return new ValidationResult("Partycode is too long",
                    new[] {nameof(TextPartyCode)});
            }

            if (TextPartyCode != null && TextPartyCode.Length < 1)
            {
                yield return new ValidationResult("Partycode is too short",
                    new[] {nameof(TextPartyCode)});
            }

            string pattern = "^[Z0-9]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(TextPartyCode ?? throw new InvalidOperationException()))
            {
                yield return new ValidationResult("Partycode should contain only digits",
                    new[] {nameof(TextPartyCode)});
            }
        }

        #endregion
    }
}
