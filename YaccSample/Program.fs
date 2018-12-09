open System
open Sql


//open System
//
//let (|InvariantEqual|_|) (str:string) arg = 
//  if String.Compare(str, arg, StringComparison.OrdinalIgnoreCase) = 0
//    then Some() else None
//
//match "HellO" with
//| InvariantEqual "hello" -> printfn "yep!"
//| _ -> printfn "Nop!" 

let x = 
    "
    UPDATE [CLT_DistribFileRepos_Errors] SET ErrorDate = (CAST(GetDate() AS FLOAT) + 2)
    FROM [CLT_DistribFileRepos_Errors]
    INNER JOIN Inserted ON
    Inserted.FileRepoId = [CLT_DistribFileRepos_Errors].FileRepoId AND
    Inserted.ClientId = [CLT_DistribFileRepos_Errors].ClientId;


    DELETE FROM CLT_DistribFileRepos_Success
    FROM CLT_DistribFileRepos_Success
    INNER JOIN Inserted ON
    CLT_DistribFileRepos_Success.FileRepoId = Inserted.FileRepoId AND
    CLT_DistribFileRepos_Success.ClientId = Inserted.ClientId;

    DECLARE @fileRepoId varchar(40);
    DECLARE repoCursor CURSOR FAST_FORWARD FOR SELECT DISTINCT FileRepoId FROM Inserted
    OPEN repoCursor;
    FETCH NEXT FROM repoCursor INTO @fileRepoId;
    WHILE(@@FETCH_STATUS = 0)
    BEGIN
      EXEC ap_DFR_UpdateConnectionStatistic @aFileRepoId = @fileRepoId;
      FETCH NEXT FROM repoCursor INTO @fileRepoId;
    END;

    CLOSE repoCursor;
    DEALLOCATE repoCursor;


  IF(@@ROWCOUNT > 0)
  BEGIN
    SET NOCOUNT ON;
    DECLARE @fileRepoId varchar(40);
    DECLARE repoCursor CURSOR FAST_FORWARD FOR SELECT DISTINCT FileRepoId FROM Deleted
    OPEN repoCursor;
    FETCH NEXT FROM repoCursor INTO @fileRepoId;
    
    WHILE(@@FETCH_STATUS = 0)
    BEGIN
      EXEC ap_DFR_UpdateConnectionStatistic @aFileRepoId = @fileRepoId;
      FETCH NEXT FROM repoCursor INTO @fileRepoId;
    END;
  
    CLOSE repoCursor;
    DEALLOCATE repoCursor;;;
    SET NOCOUNT OFF;
  END;


    -- ======================
    DELETE FROM SYS_Script_Timings 
    WHERE TimingID IN (SELECT ContainerID FROM deleted);;;;;;

        SELECT a FROM t2 WITH (readcommitted)

    INSERT INTO SYS_DeletedContainerClients (ClientID)
		SELECT Items.LinkID
		FROM CLT_CONTAINER_ITEMS AS Items with (readuncommitted) INNER JOIN Deleted
		ON Deleted.IntContainerID = Items.IntContainerID
		WHERE ((Items.ItemType = 1) AND NOT EXISTS(
			SELECT DCC.ClientID FROM SYS_DeletedContainerClients DCC
			WHERE DCC.ClientID = Items.LinkID
		));

        SET ANSI_NULLS ON
        SET QUOTED_IDENTIFIER ON

	WITH ContainerPriorities (ContainerID, IntPriority)
	AS
	(
		SELECT ContainerID, ROW_NUMBER() OVER (Order by IntPriority)
			FROM CLT_CONTAINER_STRUCTURE
	)

	UPDATE CLT_CONTAINER_STRUCTURE 
    SET IntPriority = ContainerPriorities.IntPriority
	FROM ContainerPriorities
	WHERE ContainerPriorities.ContainerID = CLT_CONTAINER_STRUCTURE.ContainerID    
    
    



		  DELETE FROM CLT_CONTAINER_ITEMS  
		  FROM CLT_CONTAINER_ITEMS, deleted d  
		  WHERE (CLT_CONTAINER_ITEMS.LinkID = d.ClientID)  
          -- single line comment
        DECLARE @helloworld int;
        DECLARE @helloworld2 nvarchar(maximum);
    
        SELECT a FROM t2
        GROUP BY asdf
        HAVING count > 1
        ORDER BY x;;;

        /* multi line comment */
       

        DECLARE @helloworld int;
        DECLARE @helloworld2 nvarchar(maximum)
        DECLARE @helloworld2 nvarchar(maximum)
         /* some /* nested 
         ulti line*/ comment */
        SELECT a, b, c
        FROM [t2]
        JOIN x ON \"x\" = x

        

SET NOCOUNT ON  

        

      IF (@@ROWCOUNT > 0 )  
      BEGIN  
		  
        SELECT x.asdf.asasdew, y, z, (SELECT 1 FROM x) AS d
        FROM t1
        LEFT JOIN t2.asdf  ON t3.ID = t2.ID
        INNER JOIN t3 ON t3.ID = t2.ID
        WHERE x = 50 AND y = 20
        ORDER BY x ASC, y DESC, z

      END  
    "   
    //@@Rowcount > 0
    //
    //

//
//        SELECT a FROM t2
//        ORDER BY x
//        GROUP BY asdf
//        HAVING count > 1

//    SELECT x, y, z   
//    FROM t1   
//    LEFT JOIN t2   
//    INNER JOIN t3 ON t3.ID = t2.ID   
//    WHERE x = 50 AND y = 20   
//    ORDER BY x ASC, y DESC, z   
//
//    SELECT a FROM t2
//    ORDER BY x
//
//    SELECT a, b, c 
//    FROM t2
//    JOIN x ON x = x

//let lexbuf = Lexing.LexBuffer<_>.FromString x
//let y = SqlParser.start SqlLexer.tokenize lexbuf   





//use inputChannel = new StreamReader(File.OpenRead tempFileName)
//let lexbuf = Lexing.LexBuffer<_>.FromTextReader inputChannel
let lexbuf = Lexing.LexBuffer<_>.FromString x
try
    let ast = SqlParser.start SqlLexer.tokenize lexbuf
    printfn "%A" ast
with e ->
    let pos = lexbuf.EndPos
    let line = pos.Line
    let column = pos.Column
    let message = e.Message
    let lastToken = new System.String(lexbuf.Lexeme)
    printf "Parse failed at line %d, column %d:\n" line column
    printf "Last token: %s" lastToken
    printf "\n"
    //exit 1   

//ast |> List.map (fun statement -> 
//                     printfn "------------------------------------"
//                     printfn "%A" statement) |> ignore

Console.WriteLine("(press any key)")   
Console.ReadKey(true) |> ignore