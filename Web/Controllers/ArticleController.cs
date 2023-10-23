using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Web.Data;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    public class ArticleController : Controller
    { private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddComment(Comment comments,int articleId, string comment)
        {
            try
            {
                comments.ArticleId = articleId;
                comments.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                comments.Comments = comment;


                _context.Comments.Add(comments);
                _context.SaveChanges();
                return RedirectToAction("Detail", "Article", new { id = articleId });
            }
            catch (Exception)
            {

                return RedirectToAction("Index");

            }
        }
        public IActionResult Detail(int id)
        {
            try
            {
                var article = _context.Articles.Include(x=>x.User).Include(x=>x.ArticleTags).ThenInclude(x=>x.Tag).SingleOrDefault(x => x.Id == id);
                var topArticles = _context.Articles.OrderByDescending(z => z.Views).Take(3).ToList();
                var NextArticle = _context.Articles.Where(x=>x.Id==id).Skip(1).FirstOrDefault();
                var PrevArticle = _context.Articles.SingleOrDefault(x => x.Id < id);

                if (article == null)
                {
                    
                    return NotFound();
                }

                var comments = _context.Comments.Include(x => x.User).Where(x => x.ArticleId == id).ToList();
                DetailVM vm = new()
                {
                    Article = article,
                    topArticles = topArticles,
                    Comments = comments,
                    PrevArticle = PrevArticle,
                    NextArticle = NextArticle

                    
                };

                return View(vm);
            } 
            catch (Exception ex)
            {
                return NotFound();
            }
        }/*Msg 3702, Level 16, State 3, Line 1
        Cannot drop database “MyDBName” because it is currently in use.
        The reason was very simple as my database was in use by another session or window.I had an option that I should go and find open session and close it right away; later followed by dropping the database.As I was in a rush I quickly wrote down following code and I was able to successfully drop the database.
        USE MASTER
        GO
        ALTER DATABASE MyDBName
        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE MyDBName
        GO*/
    } 
}
