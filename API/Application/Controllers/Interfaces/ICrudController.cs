using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Interfaces
{
    public interface ICrudController<T> where T : class
    {
        public Task<IActionResult> GetAll();
        public Task<IActionResult> Get([FromRoute] int id);
        public Task<IActionResult> Add([FromBody] T value);
        public Task<IActionResult> Edit([FromBody] T value);
        public Task<IActionResult> Delete([FromRoute] int id);
    }
}
