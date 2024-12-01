using Microsoft.AspNetCore.Mvc;

namespace Clicker.Results;

public class ErrorResult(string Message) : ActionResult;