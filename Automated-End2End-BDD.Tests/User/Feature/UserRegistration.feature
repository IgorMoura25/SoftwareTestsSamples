Feature: User - Registration
	As a visitor
	I want to register myself as a User
	So I can use the functionalities of the website

Scenario: Register user successfully
	Given the visitor is browsing the website
	When he clicks in Create Your Account
	And fill the form with data
			| Data                  |
			| E-mail                |
			| Password              |
			| Password Confirmation |
	And click in Register button
	Then he will be redirected to the catalog
	And his e-mail will appear in the top right menu

Scenario: Register without Upper Case letter in password
	Given the visitor is browsing the website
	When he clicks in Create Your Account
	And fill the form with data with a password without Upper Case letter
			| Data                  |
			| E-mail                |
			| Password              |
			| Password Confirmation |
	Then the Register button will not be able to be clicked
	And an error message will appear showing that the password must contain an Upper Case letter

Scenario: Register without special character in password
	Given the visitor is browsing the website
	When he clicks in Create Your Account
	And fill the form with data with a password without special character
			| Data                  |
			| E-mail                |
			| Password              |
			| Password Confirmation |
	Then the Register button will not be able to be clicked
	And an error message will appear showing that the password must contain a special character

## Continuar com os use cases de cadastro de usuário...
## sendo eles happy path ou não
