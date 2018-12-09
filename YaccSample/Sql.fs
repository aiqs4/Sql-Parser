// Sql.fs
module Sql

type transaction = 
    | BeginTran    of string option * string option
    | CommitTran   of string option
    | CommitWork        
    | RollBackWork
    | RollBackTran of string option
    | SaveTran     of string

type cursorStmt =  //TODO: not fully implemented yet
    | CursorDecl     of string
    | OpenCursor     of string
    | CloseCursor    of string
    | ReleaseCursor  of string
    | Fetch          of string

type tableHint = //TODO: not yet returned from grammer
    | ForceSeek
    | ForceScan
    | NoLock
    | Index
    | KeepIdentity
    | KeepDefaults 
    | FastFirstRow
    | HoldLock
    | IgnoreConstraints
    | IgnoreTriggers
    | NoWait
    | PagLock
    | ReadCommitted
    | ReadcommittedLock
    | ReadUnCommitted
    | ReadPast
    | ReapeatAbleRead
    | RowLock
    | Serializable
    | TabLock
    | TabLockX 
    | UpdLock
    | XLock

type triggerAction = 
    | Insert
    | Update
    | Delete

type variable = 
    | Var     of string //TODO: check if F# optimize this!?

type aliasedIdentifier = 
    { Identifier : string;
      Alias      : string option }     



type compareOp = 
    | Eq 
    | Gt 
    | Ge 
    | Lt
    | NGt
    | NLt 
    | Le 
    | Ne   // =, >, >=, <, <=, <> or !=
    | Like
    | In

type boolOp = 
    | And
    | Or
    | Not

type bitOp =
    | BitOr
    | BitAnd
    | BitNot
    | BitXor

type stringOp =
    | Concat //TODO: i think it's good if we group strings seperatly. to get more type-safety into the grammer


type orderBy = string * dir
and dir = Asc | Desc

type top =
    { Top: int;
      Percent: bool }

type aliasedWindowFunction = //TODO: refactor types if needed
    { WindowFunction: windowFunction;
      Alias: string option }
and windowFunction =
    | Rank      of overClause
    | NTile     of int * overClause
    | DenseRank of overClause
    | RowNumber of overClause
and overClause = 
    | RankingWindow   of rankingWindow
    | AggregateWindow of aggregateWindow //TODO: not yet implemented
and rankingWindow =
    { PartitionBy: partitionBy option;
      OrderBy: orderBy list }
and aggregateWindow = partitionBy option //TODO: not yet implemented
and partitionBy = string list

type query =
    { TableSource : tableSource;
      QueryOptions: queryOptions;
      Columns : queryColumn list;
      //Joins   : join list;
      QueryModifiers : queryModifiers }
and queryColumn = 
    | QueryColumn    of aliasedValueExpr
    | WindowFunction of aliasedWindowFunction
//    | Column     of aliasedIdentifier //TODO: implement alias in type
//    | Constant   of value //TODO: implement alias in type
//    | SubQuery   of query //TODO: implement alias in type
and tableSource =
    { Tables: aliasedIdentifier list
      Joins: join list }
and joinType = 
    | Inner 
    | Left 
    | Right 
    | Full 
    | Cross 
    | CrossApply 
    | OuterApply  
and join = aliasedIdentifier * joinType * condition option    // table name, join, optional "on" clause   
and queryModifiers =
    { Where   : condition option;
      GroupBy : groupBy option;
      OrderBy : orderBy list }
and queryOptions =
    { Top: top option;
      Distinct: bool }
and groupBy =
    { GroupBy : string list;
      Having  : condition option }

and insert = 
    { Table: string;
      Top: top option
      Columns: string list option; 
      Values: insertValues }
and insertValues =
    | ValueList    of valueExpr list
    | InsertDefaults
    | InsertQuery  of query
    | ExecResult //of execStmt //TODO: not yet implemented
and update = 
    { Table: string
      TableSource: tableSource option
      UpdateAssignments: updateAssignment list
      Where: condition option
      Top: top option 
      TableHintLimited: tableHint list option }//TODO: move tableHint and Table to a seperate type
and updateAssignment = 
    { Field: string; Value: valueExpr; }
//and updateExpr = //TODO: seems that the base, if readable could be exported. be carefull
//    | Assignment of updateAssignment
//    | Subquery   of query
//    | ConstExpr  of constExpr //TODO: 
//    | Id         of string
and delete = 
    { Table: string
      TableSource: tableSource option
      Where: condition option
      Top: top option
      TableHintLimited: tableHint list option }
and commonTableExpr = //TODO: implement hints
    { Name: string; 
      Columns: string list option;
      Query: query }
and executeStmt =
    | ExecProc   of execProc
    | ExecSql    of ExecSql
and ExecSql = 
    { Sql: string list } //TODO: this could be a concated string
and execProc = 
    { Procedure: string; 
      Parameter: param list
      Result: string option }
and param =
    { Name: string option
      Value: value } 
and condition = 
    | Cond   of (valueExpr * compareOp * valueExpr) //TODO: valueExpr has sub-query's which not ever are correct
    | And    of condition * condition
    | Or     of condition * condition
    | Exists of query



and constExpr =
    //| BaseConst of string //TODO: this could be a lot of types...
    //| Id //TODO: it could be possible, that Identifier move here
    | ConstExpr    of (value * compareOp * value)
    | BoolExpr     of (value * boolOp * value)
    | BitExpr      of (value * bitOp * value)
    | StringExpr   of (value * stringOp * value)//TODO: add missing operations
    //| CaseExpr
    //| _ //TODO: there could be more of this
    
and value = //TODO: is this really needed? i see pro and cons
    | Int        of int  
    | Float      of float  
    | String     of string
    | Identifier of string
    | Variable   of string
    | Null
and valueExpr =
    | Value          of value
    | QueryValueExpr of query //TODO: rename?
and aliasedValueExpr = 
    { Value: valueExpr
      Alias: string option } //TODO: Alias should be a own type, e.g. quoted
type varDecl = 
    { Name: string; 
      Type: string;
      Length: string option }


type sessionVar = 
    | NoCount          of bool
    //| AnsiWarnings of bool //TODO: not yet implemented
    //| AnsiPadding  of bool //TODO: not yet implemented
    //| AnsiQuotes   of bool //TODO: not yet implemented
    | AnsiNulls        of bool
    | QuotedIdentifier of bool




//type sqlStatementBlock =
//    | Stmt     of sqlStatement
//    | StmtList of sqlStatementBlock list
type sqlStatement =     
    | VarDecl       of varDecl
    | DmlStmtCTE    of dmlStmtCTE    
    | DmlStmt       of dmlStmt
    | IfStmt        of ifStmt
    | WhileStmt     of whileStmt
    | TryCatch      of tryCatch
    | CursorStmt    of cursorStmt
    | SetSessionVar of sessionVar
    | TranAction    of transaction
    | ExecuteStmt   of executeStmt
and dmlStmtCTE = 
    { Stmt: dmlStmt
      CTE: commonTableExpr list}
and dmlStmt = 
    | Query      of query 
    | Insert     of insert
    | Update     of update
    | Delete     of delete
    //| Merge    of merge
and ifStmt =    
    { Expr: condition; //TODO: rename type name
      If: sqlStatement list;
      Else: sqlStatement list option; } // list option; }
and whileStmt = 
    { Expr: condition; //TODO: rename type name
      Stmts: sqlStatement list; } // list; } //TODO: implement expression!
and tryCatch = 
    { TryBlock: sqlStatement list
      CatchBlock: sqlStatement list}
        
type batchBlock<'a> = 
    | DmlBlock of sqlStatement list
    | DdlBlock of ddlStmt
and ddlStmt = 
    | CreateTrigger    of string
    | CreateProcedure  of string
    | CreateView       of string
    | CreateTable      of string
    | CreateType       of string
    | CreateFunction   of string
    | DropTrigger      of string
    | DropProcedure    of string
    | DropView         of string
    | DropTable        of string
    | DropType         of string
    | DropFunction     of string
    | AlterTrigger     of string
    | AlterProcedure   of string
    | AlterView        of string
    | AlterTable       of string
    | AlterType        of string
    | AlterFunction    of string
