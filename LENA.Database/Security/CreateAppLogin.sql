-- Creates the least-privilege application login and user used by the API.
-- The password is supplied by the sqlcmd variable AppPassword at apply time.
-- Safe to re-run: CREATE OR ALTER keeps the login/user in sync.

USE [master];
GO

CREATE OR ALTER LOGIN [lena_app] WITH PASSWORD = '$(AppPassword)';
GO

USE [LENA];
GO

CREATE OR ALTER USER [lena_app] FOR LOGIN [lena_app];
GO

GRANT EXECUTE ON SCHEMA::[Identity] TO [lena_app];
GRANT EXECUTE ON SCHEMA::[Wine] TO [lena_app];
GRANT EXECUTE ON SCHEMA::[Inventory] TO [lena_app];
GRANT EXECUTE ON SCHEMA::[Recipe] TO [lena_app];
GRANT EXECUTE ON SCHEMA::[MealPlan] TO [lena_app];
GO
