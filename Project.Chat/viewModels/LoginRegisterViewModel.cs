using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.viewModels
{
    public class LoginRegisterViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _newUsername;
        private string _newPassword;
        private string _confirmPassword;
        private bool _isLoginButtonEnabled;
        private bool _isRegisterButtonEnabled;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ValidateLogin();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ValidateLogin();
            }
        }

        public string NewUsername
        {
            get => _newUsername;
            set
            {
                _newUsername = value;
                OnPropertyChanged();
                ValidateRegister();
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
                ValidateRegister();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
                ValidateRegister();
            }
        }

        public bool IsLoginButtonEnabled
        {
            get => _isLoginButtonEnabled;
            private set
            {
                _isLoginButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsRegisterButtonEnabled
        {
            get => _isRegisterButtonEnabled;
            private set
            {
                _isRegisterButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ValidateLogin()
        {
            IsLoginButtonEnabled = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ValidateRegister()
        {
            IsRegisterButtonEnabled = !string.IsNullOrWhiteSpace(NewUsername) &&
                                      !string.IsNullOrWhiteSpace(NewPassword) &&
                                      !string.IsNullOrWhiteSpace(ConfirmPassword);
        }
    }
}
