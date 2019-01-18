Use [Unpaids]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET XACT_ABORT ON
GO

PRINT 'Running PRESET_tb_Notification...'

	BEGIN TRY		
		BEGIN TRANSACTION
		
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Notification WHERE [Notification] = 'Push')
			BEGIN
				INSERT INTO dbo.tb_Notification
						(
						NotificationId,
						[Notification]
						)
				VALUES  (
						1, 
						'Push'
						);
			END	
			
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Notification WHERE [Notification] = 'SMS')
			BEGIN
				INSERT INTO dbo.tb_Notification
						(
						NotificationId,
						[Notification]
						)
				VALUES  (
						2, 
						'SMS'
						);
			END		
			
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Notification WHERE [Notification] = 'Email')
			BEGIN
				INSERT INTO dbo.tb_Notification
						(
						NotificationId,
						[Notification]
						)
				VALUES  (
						3, 
						'Email'
						);
			END	
			
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Notification WHERE [Notification] = 'Call')
			BEGIN
				INSERT INTO dbo.tb_Notification
						(
						NotificationId,
						[Notification]
						)
				VALUES  (
						4, 
						'Call'
						);
			END							
				
		COMMIT TRANSACTION	

		PRINT 'PRESET_tb_Notification ran successfully.'		
		RETURN;
	END TRY
	BEGIN CATCH
		PRINT 'Rolling back PRESET_tb_Notification'
		
		IF XACT_STATE() <> 0
		BEGIN
			ROLLBACK TRANSACTION
		END		

		DECLARE @ErrorSeverity NVARCHAR(255);
		DECLARE @ErrorState NVARCHAR(255);
		DECLARE @ErrorMessage NVARCHAR(MAX);
		DECLARE @ErrorLine NVARCHAR(10)
		
		SELECT
			@ErrorSeverity = ISNULL(ERROR_SEVERITY(), '')
			,@ErrorState = ISNULL(ERROR_STATE(), '')
			,@ErrorMessage = ISNULL(ERROR_MESSAGE(), '')
			,@ErrorLine = ISNULL(CAST(ERROR_LINE() AS NVARCHAR(10)),'');
			
		SELECT @ErrorMessage = 'PRESET_tb_Notification Failed: ' + @ErrorMessage + ' Line: ' + @ErrorLine;
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
		RETURN;
	END CATCH;