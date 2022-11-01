Feature: User - Login
	As a Registered User
	I want to login
	So I can use the functionalities of the website

Scenario: User login successfully
	Given the visitor is browsing the website
	When he clicks in Enter
	And fill the login form with data
			| Data                  |
			| E-mail                |
			| Password              |
	And click in Enter button
	Then he will be redirected to the catalog
	And his e-mail will appear in the top right menu

## Continuar com os use cases de login de usuário...
## sendo eles happy path ou não
