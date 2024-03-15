using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pr2_Danylov
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private DateTime _birthDate;
        private bool _isAdult;
        private string _sunSign;
        private string _chineseSign;
        private bool _isBirthday;
        private string _birthdayMessage;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                UpdateProceedButtonState();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                UpdateProceedButtonState();
            }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
                UpdateProceedButtonState();
            }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                UpdateProceedButtonState();
            }
        }

        public bool IsAdult
        {
            get { return _isAdult; }
            private set
            {
                _isAdult = value;
                OnPropertyChanged(nameof(IsAdult));
            }
        }

        public string SunSign
        {
            get { return _sunSign; }
            private set
            {
                _sunSign = value;
                OnPropertyChanged(nameof(SunSign));
            }
        }

        public string ChineseSign
        {
            get { return _chineseSign; }
            private set
            {
                _chineseSign = value;
                OnPropertyChanged(nameof(ChineseSign));
            }
        }

        public bool IsBirthday
        {
            get { return _isBirthday; }
            private set
            {
                _isBirthday = value;
                OnPropertyChanged(nameof(IsBirthday));
            }
        }

        public string BirthdayMessage
        {
            get { return _birthdayMessage; }
            set
            {
                _birthdayMessage = value;
                OnPropertyChanged(nameof(BirthdayMessage));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Proceed_Click(object sender, RoutedEventArgs e)
        {

            Mouse.OverrideCursor = Cursors.Wait;

            await Task.Run(() =>
            {
                CalculateAge();
                CalculateZodiacSigns();
                CheckBirthday();
            });


            Mouse.OverrideCursor = null;


            MessageBox.Show($"First Name: {FirstName}\n" +
                            $"Last Name: {LastName}\n" +
                            $"Email Address: {EmailAddress}\n" +
                            $"Date of Birth: {BirthDate.ToShortDateString()}\n" +
                            $"Is Adult: {IsAdult}\n" +
                            $"{SunSign}\n" +
                            $"{ChineseSign}\n" +
                            $"Is Birthday: {IsBirthday}\n" +
                            $"Birthday Message: {BirthdayMessage}");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProceedButtonState();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProceedButtonState();
        }

        private void CalculateAge()
        {
            if (BirthDate > DateTime.Now || BirthDate.Year < DateTime.Now.Year - 135)
            {
                MessageBox.Show("Please enter a valid birth date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime now = DateTime.Now;
            int age = now.Year - BirthDate.Year;
            if (BirthDate > now.AddYears(-age))
                age--;
            IsAdult = age >= 18;
        }


        private void CalculateZodiacSigns()
        {
            string[] westernZodiacSigns = { "Capricorn", "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn" };
            int indexWestern = (BirthDate.Month - 1 + (BirthDate.Day >= 20 ? 1 : 0)) % 12;
            SunSign = "Western Zodiac: " + westernZodiacSigns[indexWestern];

            string[] chineseZodiacSigns = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
            int chineseYear = BirthDate.Year;
            ChineseSign = "Chinese Zodiac: " + chineseZodiacSigns[chineseYear % 12];
        }

        private void CheckBirthday()
        {
            IsBirthday = BirthDate.Month == DateTime.Now.Month && BirthDate.Day == DateTime.Now.Day;

            if (IsBirthday)
            {
                BirthdayMessage = "Happy Birthday!";
            }
        }

        private void UpdateProceedButtonState()
        {
            btnProceed.IsEnabled = !string.IsNullOrWhiteSpace(FirstName) &&
                                   !string.IsNullOrWhiteSpace(LastName) &&
                                   !string.IsNullOrWhiteSpace(EmailAddress) &&
                                   BirthDate != default &&
                                   dpBirthDate.SelectedDate != null;
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}