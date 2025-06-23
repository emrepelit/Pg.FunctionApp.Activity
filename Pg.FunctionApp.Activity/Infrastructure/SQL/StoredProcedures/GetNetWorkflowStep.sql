CREATE PROCEDURE [dbo].[GetNetWorkflowStep]
    @ID NVARCHAR(255),
    @BusinessID INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO dbo.Audit (ID, BusinessID, CreatedDate) 
            VALUES (@ID, @BusinessID, GETUTCDATE());

        SELECT
            ID,
            StepName,
            Weight,
            DelayTimeInMs
                FROM [dbo].[WorkflowSteps]
                    WHERE ID = @ID
                        AND BusinessID = @BusinessID;
    END TRY

    BEGIN CATCH
        THROW;
    END CATCH
END