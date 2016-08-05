
namespace SuaveRestApi.Rest

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Web
open Suave.Successful
open Suave.Http
open Suave.Operators
open Suave.Filters
open Suave.RequestErrors
open SuaveRestApi.Db
open SuaveRestApi.Data

[<AutoOpen>]
module RestFul =
    type RestResource<'a> = {
        GetAll : unit -> Async<'a seq>
        Create: 'a -> string Result
        // Update: 'a -> 'a option
        // Delete: int -> Result 
        // GetById: int -> 'a option
        // UpdateById: int -> 'a -> 'a option
    }

    let JSON v =
        let jsonSerializerSettings = new JsonSerializerSettings()
        jsonSerializerSettings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(v, jsonSerializerSettings)
            |> OK
            >=> Writers.setMimeType "application/json; charset=utf-8"

    let fromJson<'a> json =
        JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a 

    let getResourceFromReq<'a> req = 
        let getString = System.Text.Encoding.UTF8.GetString
        req.rawForm 
            |> getString 
            |> fromJson<'a>

    let rest resourceName resource =
        let resourcePath = "/" + resourceName
        let badRequest = BAD_REQUEST "Resource not found"
        let HTTP404 = NOT_FOUND "Resource not found"

        let handleResource requestError = function
            | Some r -> r |> JSON
            | _ -> requestError
            
        let pathScanId = 
            pathScan (new PrintfFormat<int, _, _, _, _> (resourcePath + "/%d"))

        // let getById =
        //     resource.GetById >> handleResource HTTP404

        // let updateById id = 
        //     request (getResourceFromReq >> resource.UpdateById id >> handleResource badRequest)

        // let deleteById id =
        //     resource.Delete id |> function 
        //         | Success value -> NO_CONTENT
        //         | Failure msg -> HTTP404
        
        let getAll _ = 
            async {
                let! resources = resource.GetAll()
                return resources |> JSON
            } 
            
            |> Async.RunSynchronously 

        choose [
            path resourcePath >=> choose [
                GET >=> warbler getAll
                POST >=> request (getResourceFromReq >> resource.Create >> JSON)
                // PUT >=> request (getResourceFromReq >> resource.Update >> handleResource badRequest)
            ]
            // DELETE >=> pathScanId deleteById
            // GET    >=> pathScanId getById
            // PUT    >=> pathScanId updateById
        ]

    
