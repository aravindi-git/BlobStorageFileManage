using FileManageServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileManagerSrevice fileManagerSrevice;

        public FilesController(IFileManagerSrevice fileManagerSrevice)
        {
            this.fileManagerSrevice = fileManagerSrevice; 
        }

        [Route("upload")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fileURL = await fileManagerSrevice.UploadFile(file.OpenReadStream(), fileName, file.ContentType);

                    return Ok(new { fileURL });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [Route("getFiles")]
        [HttpGet]
        public IActionResult GetFiles()
        {
            try
            {
                var files = fileManagerSrevice.GetFilesList();

                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [Route("view")]
        [HttpGet]
        public async Task<IActionResult> View(string filename)
        {
            var viewFileResult = await fileManagerSrevice.ViewFile(filename);
            return File(viewFileResult.Content, viewFileResult.ContentType);
        }


        [Route("download")]
        [HttpGet, DisableRequestSizeLimit]
        public async Task<ActionResult> Download(string fileName)
        {
            var downloadResult = await fileManagerSrevice.DownloadFile(fileName);
            if(downloadResult != null)
            {
                return File(downloadResult.FileStream, downloadResult.ContentType, fileName);
            }
            else
            {
                return BadRequest();
            }
           
        }


        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string fileName)
        {
            await fileManagerSrevice.Delete(fileName);
            return Ok();
        }


    }
}
