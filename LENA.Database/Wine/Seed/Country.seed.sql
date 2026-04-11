-- Seed Countries
INSERT INTO [Wine].[Country] ([CountryName], [ISOCode], [Description], [IsActive], [CreatedBy], [CreateDate])
VALUES 
('France', 'FR', 'France', 1, 'SYSTEM', GETUTCDATE()),
('Italy', 'IT', 'Italy', 1, 'SYSTEM', GETUTCDATE()),
('Spain', 'ES', 'Spain', 1, 'SYSTEM', GETUTCDATE()),
('USA', 'US', 'United States', 1, 'SYSTEM', GETUTCDATE()),
('Australia', 'AU', 'Australia', 1, 'SYSTEM', GETUTCDATE()),
('New Zealand', 'NZ', 'New Zealand', 1, 'SYSTEM', GETUTCDATE()),
('Germany', 'DE', 'Germany', 1, 'SYSTEM', GETUTCDATE()),
('Portugal', 'PT', 'Portugal', 1, 'SYSTEM', GETUTCDATE()),
('Greece', 'GR', 'Greece', 1, 'SYSTEM', GETUTCDATE()),
('Argentina', 'AR', 'Argentina', 1, 'SYSTEM', GETUTCDATE()),
('Chile', 'CL', 'Chile', 1, 'SYSTEM', GETUTCDATE()),
('South Africa', 'ZA', 'South Africa', 1, 'SYSTEM', GETUTCDATE()),
('China', 'CN', 'China', 1, 'SYSTEM', GETUTCDATE()),
('Japan', 'JP', 'Japan', 1, 'SYSTEM', GETUTCDATE());
