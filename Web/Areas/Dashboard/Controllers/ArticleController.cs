using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Data;
using Web.Models;

namespace Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class ArticleController : Controller
    {private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles.Include(x => x.User).Include(y => y.ArticleTags).ThenInclude(z => z.Tag).ToList();
            return View(articles  );
        }
        [HttpGet]
        public IActionResult Create()
        { var tagList = _context.Tags.ToList();
            ViewBag.Tags = new SelectList(tagList, "Id", "TagName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article article, List<int> tagIds)
        {
            try
            {
                article.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.SeoUrl = "dedede";
                article.CreatedDate = DateTime.Now;
                article.UpdatedDate = DateTime.Now;
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articleTag = new()
                    {
                        TagId = tagIds[i],
                        ArticleId = article.Id
                    };
                    _context.ArticleTags.AddAsync(articleTag);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View(article);
            }
        } 
            public IActionResult Edit(int id)
            {
            
            
             var article = _context.Articles.Include(x => x.ArticleTags).SingleOrDefault(x=>x.Id == id);
             if (article == null||id == null) {return NotFound(); }
             var tags = _context.Tags.ToList();
             ViewData["taglist"] = tags;    
             return View(article);
            }
        [HttpPost]
        public IActionResult Edit(Article article,List<int>tagIds) {

            article.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            article.SeoUrl = ("SDsdsd");
            article.UpdatedDate = DateTime.Now;
            _context.Articles.Update(article);
            _context.SaveChanges();
            var articleTag = _context.ArticleTags.Where(a => a.ArticleId == article.Id).ToList();
            _context.ArticleTags.RemoveRange(articleTag);
            _context.SaveChanges();

            for (int i = 0; i < tagIds.Count; i++)
            {
                ArticleTag articletag = new()
                {
                    TagId = tagIds[i],
                    ArticleId = article.Id
                };
                _context.ArticleTags.AddAsync(articletag);
            }
            _context.SaveChangesAsync();

            _context.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
