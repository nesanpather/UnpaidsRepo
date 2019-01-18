Use [Unpaids]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET XACT_ABORT ON
GO

PRINT 'Running PRESET_tb_Response...'

	BEGIN TRY		
		BEGIN TRANSACTION
		
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Response WHERE [Response] = 'Call Me')
			BEGIN
				INSERT INTO dbo.tb_Response
						(
						ResponseId,
						[Response]
						)
				VALUES  (
						1, 
						'Call Me'
						);
			END	
			
			IF NOT EXISTS (SELECT 1 FROM dbo.tb_Response WHERE [Response] = 'Email Me')
			BEGIN
				INSERT INTO dbo.tb_Response
						(
						ResponseId,
						[Response]
						)
				VALUES  (
						2, 
						'Email Me'
						);
			END							
				
		COMMIT TRANSACTION	

		PRINT 'PRESET_tb_Response ran successfully.'		
		RETURN;
	END TRY
	BEGIN CATCH
		PRINT 'Rolling back PRESET_tb_Response'
		
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
			
		SELECT @ErrorMessage = 'PRESET_tb_Response Failed: ' + @ErrorMessage + ' Line: ' + @ErrorLine;
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
		RETURN;
	END CATCH;