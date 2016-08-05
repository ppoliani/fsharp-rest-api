namespace SuaveRestApi.Data

open MongoDB.Bson
open MongoDB.Driver
open MongoDB.FSharp

type Person = { Id:BsonObjectId; Name:string; Age:int; Email: string }

type Result<'T> = 
    | Success of 'T
    | Failure of string

[<AutoOpen>]
module PersonRepository =     
    let people = db.GetCollection<BsonDocument> "people"
    let wildcard = FilterDefinition<BsonDocument>.op_Implicit("{}")
    
    let tmpPerson = 
        {Id = BsonObjectId.Create(ObjectId.GenerateNewId())
         Name = "Pavlos"
         Age = 20
         Email = "Email"} 

    let mapDocToPerson (doc: BsonDocument) = 
        { Id = BsonObjectId.Create(doc.GetValue "_id" |> string)
          Name = doc.GetValue "Name" |> string
          Age = doc.GetValue "Age" |> int
          Email = doc.GetValue "Email" |> string}
    
    let mapPersonToDoc person = 
        BsonDocument([ BsonElement("Name", BsonString person.Name)
                       BsonElement("Age", BsonInt32 person.Age)
                       BsonElement("Email", BsonString person.Email) ])

    let getPeople() = 
        printfn "Fetching People"
        
        async {
            let! people = people.Find(wildcard).ToListAsync() |> Async.AwaitTask
            
            return people 
                |> Seq.map mapDocToPerson 
                |> List.ofSeq 
                |> Seq.ofList
        }

    let getPersonById id = None
    
    let createPerson person =
        printfn "Creating People"
        
        let personDoc = mapPersonToDoc person

        personDoc
            |> people.InsertOneAsync
            |> Async.AwaitIAsyncResult
            |> Async.RunSynchronously
            |> function 
                | true -> Success(personDoc.GetValue "_id" |> string) 
                | _    -> Failure "Could not insert"

    let updatePersonById personId personToBeUpdated = None

    let updatePerson personToBeUpdated = None

    let deletePerson personId = Success




