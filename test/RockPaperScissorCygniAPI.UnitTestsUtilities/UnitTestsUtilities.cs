using Microsoft.AspNetCore.Mvc;

namespace RockPaperScissorCygniAPI.UnitTests.Utilities
{
    public static class UnitTestsUtilities
    {
        // Helper method
        public static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}