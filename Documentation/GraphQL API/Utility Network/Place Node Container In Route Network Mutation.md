## Example Mutation

```graphql
mutation 
{
  nodeContainer
  {
    placeNodeContainerInRouteNetwork(
      routeNodeId: "020c3eb8-13cc-4c0e-a331-c7610f996a52"
      nodeContainerId: "5bf6561c-ba6e-4eb9-b6fc-494c3fa90a64"
      nodeContainerSpecificationId: "0fb389b5-4bbd-4ebf-b506-bfc636001171"
      manufacturerId: "6b02e4aa-19f1-46a5-85e8-c1faab236ef0"
    )
    {
      isSuccess
      errorCode
    }
  }
}
```

## Params
All are mandatory.


## Error Codes
[Error Codes](https://github.com/DAXGRID/open-ftth-utility-graph-service/blob/master/OpenFTTH.UtilityGraphService.API/Commands/PlaceNodeContainerInRouteNetworkErrorCodes.cs)





