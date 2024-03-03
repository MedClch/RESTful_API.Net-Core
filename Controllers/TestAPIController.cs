using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tutoriel.Data;
using Tutoriel.DTOs;
using Tutoriel.Models;

namespace Tutoriel.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")] //sepecifier que id est de type int
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(401)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id==0)
                return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id==id);
            if (villa==null)
                return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
        {
            //if(!ModelState.IsValid) return BadRequest(ModelState);
            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower()==villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Cutsom Error", "Villa already exists !");
                return BadRequest(ModelState);
            }
            if (villa==null) { return BadRequest(); }
            if (villa.Id>0) { return StatusCode(StatusCodes.Status500InternalServerError); }
            villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villa);
            //return Ok(villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id==0) { return BadRequest(); }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id==id);
            if (villa==null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO newVilla)
        {
            if(newVilla==null || id!=newVilla.Id) { 
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id==id);
            villa.Name = newVilla.Name;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartVilla(int id,JsonPatchDocument<VillaDTO> patchDocument)
        {
            if (patchDocument==null||id==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id==id);
            if (villa==null)
            {
                return BadRequest();
            }
            patchDocument.ApplyTo(villa,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }
    }
}
