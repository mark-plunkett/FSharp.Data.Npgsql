﻿#I "../../src/bin/Debug/"

#r "System.Transactions"
#r "FSharp.Data.Npgsql.dll"
#r "Npgsql.dll"
#r "System.Threading.Tasks.Extensions"
#r "System.ValueTuple.dll"

open Npgsql
open FSharp.Data
open System
open System.Data
open Npgsql.Logging

[<Literal>]
let connectionString = "Host=localhost;Username=postgres;Password=Leningrad1;Database=lims"

//NpgsqlLogManager.Provider <- ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
//NpgsqlLogManager.IsParameterLoggingEnabled <- true

do 
    let cmd = new NpgsqlCommand<"
        select * from onto.namespace where id = @id
    ", connectionString, ResultType.DataTable >(connectionString)

    let t = cmd.Execute(id = 77)

    [ for c in t.Columns -> c.ColumnName, c.DataType, c.DateTimeMode ] |> printfn "Columns: %A"

    if (t.Rows.Count > 0)
    then 
        let r = t.Rows.[0]
        r.name <- r.name + "_test"
        t.Update(conflictOption = System.Data.ConflictOption.CompareAllSearchableValues ) |> printfn "Rows affected: %i"
    
//do 
//    use conn = new NpgsqlConnection(connectionString)
//    conn.Open()
//    let cmd = new NpgsqlCommand<"select NULL", connectionString, SingleRow = true>(conn)
//    cmd.Execute() |> printfn "%A"

//type Get42 = NpgsqlCommand<"select 42 as X, current_date", connectionString>

//do 
//    let cmd = new Get42(connectionString)
//    let xs = [ for x in cmd.Execute() -> x.x, x.date ] 
//    xs |> printfn "resultset:\n%A"

//do 
//    let cmd = new NpgsqlCommand<"        
//        SELECT table_schema || '.' || table_name as name, table_type, row_number() over() as c, 43::integer as jjj
//        FROM information_schema.tables 
//        WHERE table_type = @tableType;
//        ", connectionString>(connectionString)

//    let xs = [ for x in cmd.Execute(tableType = "VIEW") -> x.name, x.table_type, x.c, x.jjj ] 
//    xs |> printfn "resultset:\n%A"

//do 
//    let cmd = new NpgsqlCommand<"
//        select * from onto.namespace
//    ", connectionString>(connectionString)
//    [ for x in cmd.Execute() -> x.id, x.description, x.name ] |> printfn "%A"

//do 
//    let cmd = new NpgsqlCommand<" 
//        select id, name from onto.namespace
//    ", connectionString, ResultType.Tuples >(connectionString)

//    cmd.Execute() |> Seq.toList |> printfn "resultset:\n%A"

//do 
//    let cmd = new NpgsqlCommand<"select count(*) as c from onto.namespace", connectionString, SingleRow = true>(connectionString)
//    cmd.Execute() |> printfn "resultset:\n%A"

//do 
//    let cmd = new NpgsqlCommand<"SELECT coalesce(@x, 'Empty') AS x", connectionString, NullableParameters = true>(connectionString)
//    cmd.Execute(Some "haha") |> printfn "resultset:\n%A"

//type TryEnums = NpgsqlCommand<"
//    SELECT 
//        'pt'::material.arc_direction as col1,
//        'tp'::material.arc_direction as col2
//", connectionString>

//do 
//    let x = TryEnums.``material.arc_direction``.pt
//    let cmd = new TryEnums(connectionString)
//    cmd.Execute() 
//    |> Seq.map (fun x -> string x.col1.Value, string x.col2.Value) 
//    |> Seq.toList
//    |> printfn "resultset:\n%A"

////type UpdateWithEnum = NpgsqlCommand<"
////    INSERT INTO part.location (coord1, coord2, is_fwd, type) 
////    VALUES (@coor1, @coor2, @is_fwd , @type::part.sbol_location_type)
////", connectionString>

////do  
////    use cmd = UpdateWithEnum.Create(connectionString)
////    cmd.Execute(12, 12, true, UpdateWithEnum.``part.sbol_location_type``.cut) |> printfn "Records inserted %i"

//open System
//do
//    use cmd = new NpgsqlCommand<"
//        INSERT INTO part.gsl_source_doc (id_user, title, content, favorite, parents, locked, created, updated) 
//        VALUES (@id_user, @title, @content, @favorite, ARRAY[ @parent1::int, @parent2::int ], true, @created, @updated) 
//        RETURNING id
//    ", connectionString, SingleRow = true>(connectionString)
//    cmd.Execute(1,  "test title", "test content", true, 1, 2, DateTimeOffset.Now, DateTimeOffset.Now ) |> printfn "Records inserted %A"
    

