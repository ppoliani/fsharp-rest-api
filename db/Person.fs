namespace SuaveRestApi.Data

open MongoDB.Bson
open MongoDB.Driver
open MongoDB.FSharp

type Person = { Id:BsonObjectId; Name:string; Age:int; Email: string }

type Result = 
    | Success
    | Failure

[<AutoOpen>]
module PersonRepository =     
    let people = db.GetCollection<BsonDocument> "people"
    let wildcard = FilterDefinition<BsonDocument>.op_Implicit("{}")
    
    let tmpPerson = 
        {Id = BsonObjectId(ObjectId.GenerateNewId())
         Name = "Pavlos"
         Age = 20
         Email = "Email"} 

    let mapDocToPerson doc = doc
    
    let getPeople() = 
        printfn "Fetching People"
        
        people.Find(wildcard).ToListAsync() 
        |> Async.AwaitTask 
        |> Async.RunSynchronously 
        |> Seq.map mapDocToPerson
        |> List.ofSeq
        |> Seq.ofList

    let getPersonById id = None
    
    let createPerson person = tmpPerson

    let updatePersonById personId personToBeUpdated = None

    let updatePerson personToBeUpdated = None

    let deletePerson personId = Success




