Use [Unpaids]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET XACT_ABORT ON
GO

PRINT 'Running PRESET_tb_Status...'

	BEGIN TRY		
		BEGIN TRANSACTION
		
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Status WHERE [Status] = 'Pending')
			BEGIN
				INSERT INTO dbo.tb_Status
						(
						StatusId,
						[Status]
						)
				VALUES  (
						1, 
						'Pending'
						);
			END	
			
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Status WHERE [Status] = 'Success')
			BEGIN
				INSERT INTO dbo.tb_Status
						(
						StatusId,
						[Status]
						)
				VALUES  (
						2, 
						'Success'
						);
			END	
			
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Status WHERE [Status] = 'Failed')
			BEGIN
				INSERT INTO dbo.tb_Status
						(
						StatusId,
						[Status]
						)
				VALUES  (
						3, 
						'Failed'
						);
			END					
				
		COMMIT TRANSACTION	

		PRINT 'PRESET_tb_Status ran successfully.'		
		RETURN;
	END TRY
	BEGIN CATCH
		PRINT 'Rolling back PRESET_tb_Status'
		
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
			
		SELECT @ErrorMessage = 'PRESET_tb_Status Failed: ' + @ErrorMessage + ' Line: ' + @ErrorLine;
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
		RETURN;
	END CATCH;