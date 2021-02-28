PRINT 'Before TRY'
BEGIN TRY
	BEGIN TRAN
	PRINT 'First Statement in the TRY block'
IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210228015859_DomainEventOutbox')
BEGIN
    CREATE TABLE [dbo].[Outbox] (
        [MessageId] nvarchar(450) NOT NULL,
        [CorrelationId] nvarchar(max) NULL,
        [EventType] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [Body] nvarchar(max) NULL,
        [Status] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [ScheduledDate] datetime2 NOT NULL,
        [PublishedDate] datetime2 NULL,
        [LockId] nvarchar(max) NULL,
        CONSTRAINT [PK_Outbox] PRIMARY KEY ([MessageId])
    );
END;


IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210228015859_DomainEventOutbox')
BEGIN
    CREATE INDEX [IX_ScheduleDate_Status] ON [dbo].[Outbox] ([ScheduledDate], [Status]) INCLUDE ([EventType]);
END;


IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210228015859_DomainEventOutbox')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210228015859_DomainEventOutbox', N'3.1.12');
END;


	PRINT 'Last Statement in the TRY block'
	COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'In CATCH Block'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;

    THROW; -- Raise error to the client.
END CATCH
PRINT 'After END CATCH'
GO
