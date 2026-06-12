using MonTrack.Auth.Api;

namespace MonTrack.Mobile;

public partial class LoginPage : ContentPage
{
	private AuthApiSimulator _authApi = null!;

	public LoginPage()
	{
		InitializeComponent();
		_authApi = new AuthApiSimulator();

		// Seed users for testing
		_authApi.Register("user@test.com", "password123");
		_authApi.Register("admin1@gmail.com", "admin123");
	}

	private bool _isSignUpMode = false;

	private void ToggleMode_Tapped(object sender, EventArgs e)
	{
		_isSignUpMode = !_isSignUpMode;
		ConfirmPasswordStack.IsVisible = _isSignUpMode;
		BtnLogin.Text = _isSignUpMode ? "Sign Up Now" : "Login Now";
		FooterTextLabel.Text = _isSignUpMode ? "Already have an account?" : "Don't have an account?";
		FooterActionLabel.Text = _isSignUpMode ? "Login" : "Sign Up";
		StatusLabel.Text = "";
		EmailEntry.Text = "";
		PasswordEntry.Text = "";
		ConfirmPasswordEntry.Text = "";
	}

	private async void BtnLogin_Clicked(object sender, EventArgs e)
	{
		string email = EmailEntry.Text;
		string password = PasswordEntry.Text;

		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
		{
			StatusLabel.TextColor = Color.FromArgb("#FF7675");
			StatusLabel.Text = "Please fill in all fields.";
			return;
		}

		if (_isSignUpMode)
		{
			string confirmPassword = ConfirmPasswordEntry.Text;
			if (string.IsNullOrEmpty(confirmPassword))
			{
				StatusLabel.TextColor = Color.FromArgb("#FF7675");
				StatusLabel.Text = "Please confirm your password.";
				return;
			}
			if (password != confirmPassword)
			{
				StatusLabel.TextColor = Color.FromArgb("#FF7675");
				StatusLabel.Text = "Passwords do not match.";
				return;
			}

			BtnLogin.IsEnabled = false;
			BtnLogin.Text = "Registering...";
			StatusLabel.Text = "";

			var response = await Task.Run(() => _authApi.Register(email, password));

			if (response.IsSuccess)
			{
				StatusLabel.TextColor = Color.FromArgb("#55EFC4");
				StatusLabel.Text = "Registration successful! You can now login.";
				
				// Switch back to Login mode automatically
				_isSignUpMode = false;
				ConfirmPasswordStack.IsVisible = false;
				BtnLogin.Text = "Login Now";
				FooterTextLabel.Text = "Don't have an account?";
				FooterActionLabel.Text = "Sign Up";
				ConfirmPasswordEntry.Text = "";
			}
			else
			{
				StatusLabel.TextColor = Color.FromArgb("#FF7675");
				StatusLabel.Text = response.Message;
			}

			BtnLogin.IsEnabled = true;
			if (!_isSignUpMode)
				BtnLogin.Text = "Login Now";
			else
				BtnLogin.Text = "Sign Up Now";
		}
		else
		{
			BtnLogin.IsEnabled = false;
			BtnLogin.Text = "Authenticating...";
			StatusLabel.Text = "";

			// Trigger State Machine via API Simulator
			var response = await Task.Run(() => _authApi.Login(email, password));

			if (response.IsSuccess)
			{
				StatusLabel.TextColor = Color.FromArgb("#55EFC4");
				StatusLabel.Text = "Access Granted!";
				
				// Navigation transition to Dashboard (Standard .NET 9)
				await Task.Delay(500);
				if (Application.Current?.Windows.Count > 0)
				{
					Application.Current.Windows[0].Page = new NavigationPage(new MainPage());
				}
			}
			else
			{
				StatusLabel.TextColor = Color.FromArgb("#FF7675");
				StatusLabel.Text = response.Message;
				BtnLogin.IsEnabled = true;
				BtnLogin.Text = "Login Now";
			}
		}
	}
}
