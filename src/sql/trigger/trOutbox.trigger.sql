DROP TRIGGER IF EXISTS trOutbox
GO

---
-- Trigger for Outbox that will handle logging of both update and delete
-- NOTE: inserted data is current value in row if not altered
---
CREATE TRIGGER trOutbox
	ON [dbo].[Outbox]
	FOR UPDATE, DELETE
	AS
		BEGIN
	SET NOCOUNT ON

	DECLARE 
		@AuditLogTransactionId	int,
		@Inserted	    		int = 0,
 		@ROWS_COUNT				int

	SELECT @ROWS_COUNT=count(*) from inserted

    -- Check if this is an INSERT, UPDATE or DELETE Action.
    DECLARE @action as varchar(10);
    SET @action = 'INSERT';
    IF EXISTS(SELECT 1 FROM DELETED)
    BEGIN
        SET @action = 
            CASE
                WHEN EXISTS(SELECT 1 FROM INSERTED) THEN 'UPDATE'
                ELSE 'DELETE'
            END
    END

	-- determine username
	DECLARE @UserName nvarchar(200);
	SET @UserName = CURRENT_USER

	-- insert parent transaction
	INSERT INTO audit.AuditLogTransaction (TableName, TableSchema, Action, HostName, ApplicationName, AuditLogin, AuditDate, AffectedRows, DatabaseName, UserId, TransactionId)
	values('Outbox', 'dbo', @action, CASE WHEN LEN(HOST_NAME()) < 1 THEN ' ' ELSE HOST_NAME() END,
		CASE WHEN LEN(APP_NAME()) < 1 THEN ' ' ELSE APP_NAME() END,
		SUSER_SNAME(), GETDATE(), @ROWS_COUNT, db_name(), @UserName, CURRENT_TRANSACTION_ID()
	)
	Set @AuditLogTransactionId = SCOPE_IDENTITY()

		-- [MessageId]
	IF UPDATE([MessageId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[MessageId]',
				CONVERT(nvarchar(4000), OLD.[MessageId], 126),
				CONVERT(nvarchar(4000), NEW.[MessageId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[MessageId] <> OLD.[MessageId]) 
					Or (NEW.[MessageId] Is Null And OLD.[MessageId] Is Not Null)
					Or (NEW.[MessageId] Is Not Null And OLD.[MessageId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [CorrelationId]
	IF UPDATE([CorrelationId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[CorrelationId]',
				CONVERT(nvarchar(4000), OLD.[CorrelationId], 126),
				CONVERT(nvarchar(4000), NEW.[CorrelationId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[CorrelationId] <> OLD.[CorrelationId]) 
					Or (NEW.[CorrelationId] Is Null And OLD.[CorrelationId] Is Not Null)
					Or (NEW.[CorrelationId] Is Not Null And OLD.[CorrelationId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [EventType]
	IF UPDATE([EventType]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[EventType]',
				CONVERT(nvarchar(4000), OLD.[EventType], 126),
				CONVERT(nvarchar(4000), NEW.[EventType], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[EventType] <> OLD.[EventType]) 
					Or (NEW.[EventType] Is Null And OLD.[EventType] Is Not Null)
					Or (NEW.[EventType] Is Not Null And OLD.[EventType] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Topic]
	IF UPDATE([Topic]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[Topic]',
				CONVERT(nvarchar(4000), OLD.[Topic], 126),
				CONVERT(nvarchar(4000), NEW.[Topic], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[Topic] <> OLD.[Topic]) 
					Or (NEW.[Topic] Is Null And OLD.[Topic] Is Not Null)
					Or (NEW.[Topic] Is Not Null And OLD.[Topic] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [RoutingKey]
	IF UPDATE([RoutingKey]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[RoutingKey]',
				CONVERT(nvarchar(4000), OLD.[RoutingKey], 126),
				CONVERT(nvarchar(4000), NEW.[RoutingKey], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[RoutingKey] <> OLD.[RoutingKey]) 
					Or (NEW.[RoutingKey] Is Null And OLD.[RoutingKey] Is Not Null)
					Or (NEW.[RoutingKey] Is Not Null And OLD.[RoutingKey] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Body]
	IF UPDATE([Body]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[Body]',
				CONVERT(nvarchar(4000), OLD.[Body], 126),
				CONVERT(nvarchar(4000), NEW.[Body], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[Body] <> OLD.[Body]) 
					Or (NEW.[Body] Is Null And OLD.[Body] Is Not Null)
					Or (NEW.[Body] Is Not Null And OLD.[Body] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Status]
	IF UPDATE([Status]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[Status]',
				CONVERT(nvarchar(4000), OLD.[Status], 126),
				CONVERT(nvarchar(4000), NEW.[Status], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[Status] <> OLD.[Status]) 
					Or (NEW.[Status] Is Null And OLD.[Status] Is Not Null)
					Or (NEW.[Status] Is Not Null And OLD.[Status] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [CreatedDate]
	IF UPDATE([CreatedDate]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[CreatedDate]',
				CONVERT(nvarchar(4000), OLD.[CreatedDate], 126),
				CONVERT(nvarchar(4000), NEW.[CreatedDate], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[CreatedDate] <> OLD.[CreatedDate]) 
					Or (NEW.[CreatedDate] Is Null And OLD.[CreatedDate] Is Not Null)
					Or (NEW.[CreatedDate] Is Not Null And OLD.[CreatedDate] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [ScheduledDate]
	IF UPDATE([ScheduledDate]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[ScheduledDate]',
				CONVERT(nvarchar(4000), OLD.[ScheduledDate], 126),
				CONVERT(nvarchar(4000), NEW.[ScheduledDate], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[ScheduledDate] <> OLD.[ScheduledDate]) 
					Or (NEW.[ScheduledDate] Is Null And OLD.[ScheduledDate] Is Not Null)
					Or (NEW.[ScheduledDate] Is Not Null And OLD.[ScheduledDate] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [PublishedDate]
	IF UPDATE([PublishedDate]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[PublishedDate]',
				CONVERT(nvarchar(4000), OLD.[PublishedDate], 126),
				CONVERT(nvarchar(4000), NEW.[PublishedDate], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[PublishedDate] <> OLD.[PublishedDate]) 
					Or (NEW.[PublishedDate] Is Null And OLD.[PublishedDate] Is Not Null)
					Or (NEW.[PublishedDate] Is Not Null And OLD.[PublishedDate] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [LockId]
	IF UPDATE([LockId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[MessageId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[MessageId], NEW.[MessageId]), 0), '[[MessageId]] Is Null')),
				'[LockId]',
				CONVERT(nvarchar(4000), OLD.[LockId], 126),
				CONVERT(nvarchar(4000), NEW.[LockId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[MessageId], NEW.[MessageId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[MessageId] = OLD.[MessageId] or (NEW.[MessageId] Is Null and OLD.[MessageId] Is Null))
			WHERE ((NEW.[LockId] <> OLD.[LockId]) 
					Or (NEW.[LockId] Is Null And OLD.[LockId] Is Not Null)
					Or (NEW.[LockId] Is Not Null And OLD.[LockId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END



	--IF @Inserted = 0
	--	BEGIN
	--	    -- believed to be contributing to deadlocks
	--		-- DELETE FROM audit.AuditLogTransaction WHERE AuditLogTransactionId = @AuditLogTransactionId
	--	END
END
GO
