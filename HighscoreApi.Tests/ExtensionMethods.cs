using System;
using HighscoreApi.Dto;
using Microsoft.AspNetCore.Mvc;

public static class ActionResultExtensions
{
  public static T Get<T>(this ActionResult<T> actionResult)
  {
    if (actionResult.Result is CreatedAtRouteResult)
    {
      var created = (CreatedAtRouteResult)actionResult.Result;
      return (T)created.Value;
    }
    else if (actionResult.Result is ObjectResult)
    {
      var created = (ObjectResult)actionResult.Result;
      return (T)created.Value;
    }
    else
    {
      throw new Exception("Unable to get object of type" + typeof(T));
    }
  }
}