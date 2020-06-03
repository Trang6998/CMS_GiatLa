using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using HVITCore.Controllers;
using System.Data.Entity.Infrastructure;
using CMS.Models;

namespace CMS.Controllers
{
    [RoutePrefix("api/coso")]
    public class CoSoController : BaseApiController
    {
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Search([FromUri]Pagination pagination, [FromUri]string keywords = null)
        {
            using (var db = new ApplicationDbContext())
            {
                IQueryable<CoSo> results = db.CoSo;
                if (pagination == null)
                    pagination = new Pagination();
                if (pagination.includeEntities)
                {
                    
                }

                if (!string.IsNullOrWhiteSpace(keywords))
                    results = results.Where(x => x.TenCoSo.Contains(keywords));

                results = results.OrderBy(o => o.CoSoID);

                return Ok((await GetPaginatedResponseAsync(results, pagination)));
            }
        }

        [HttpGet, Route("{coSoID:int}")]
        public async Task<IHttpActionResult> Get(int coSoID)
        {
            using (var db = new ApplicationDbContext())
            {
                var coSo = await db.CoSo
                    .SingleOrDefaultAsync(o => o.CoSoID == coSoID);

                if (coSo == null)
                    return NotFound();

                return Ok(coSo);
            }
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Insert([FromBody]CoSo coSo)
        {
            if (coSo.CoSoID != 0) return BadRequest("Invalid CoSoID");

            using (var db = new ApplicationDbContext())
            {
                db.CoSo.Add(coSo);
                await db.SaveChangesAsync();
            }

            return Ok(coSo);
        }

        [HttpPut, Route("{coSoID:int}")]
        public async Task<IHttpActionResult> Update(int coSoID, [FromBody]CoSo coSo)
        {
            if (coSo.CoSoID != coSoID) return BadRequest("Id mismatch");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var db = new ApplicationDbContext())
            {
                db.Entry(coSo).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ducEx)
                {
                    bool exists = db.CoSo.Count(o => o.CoSoID == coSoID) > 0;
                    if (!exists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ducEx;
                    }
                }

                return Ok(coSo);
            }
        }

        [HttpDelete, Route("{coSoID:int}")]
        public async Task<IHttpActionResult> Delete(int coSoID)
        {
            using (var db = new ApplicationDbContext())
            {
                var coSo = await db.CoSo.SingleOrDefaultAsync(o => o.CoSoID == coSoID);

                if (coSo == null)
                    return NotFound();

                db.Entry(coSo).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return Ok();
            }
        }

    }
}
