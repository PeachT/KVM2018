using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KVM.entity;
using KVM.LiteDB.DAL.Project;
using KVM.LiteDB.DAL.Project.Supervision;
using DeviceClient.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeviceClient.Controllers
{
  [Route("api/[controller]")]
  public class SupervisionController : Controller
  {
    public ISupervision _col;
    private string rootPath;
    public SupervisionController([FromServices]IHostingEnvironment env, ISupervision col)
    {
      rootPath = env.WebRootPath;
      _col = col;
    }
    // GET: /<controller>/
    public IActionResult Index()
    {
      return Json(_col.GetAll());
    }

    [HttpPost]
    public IActionResult Post(Supervision data)
    {
      data.Id = Guid.NewGuid().ToString();
      data.ImgUrl = FileOperation.FileImg(data.ImgFile, data.Id, rootPath, Path(data.ParentId));
      return Json(_col.Insert(data));
    }

    [HttpPut("{id}")]
    public IActionResult Put(string id, Supervision data)
    {
      data.ImgUrl = FileOperation.FileImg(data.ImgFile, data.Id, rootPath, Path(data.ParentId));
      return Json(_col.UpData(id, data));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
      var delete = _col.GetOne(id);
      FileOperation.DeleteFile(rootPath, delete.ImgUrl);
      return Json(_col.Delete(delete));
    }

    private string Path(string path)
    {
      return $@"data/project/Supervision/{path}";
    }
  }
}
