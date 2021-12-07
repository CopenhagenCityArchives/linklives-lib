using Linklives.DAL;
using Linklives.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace linklives_lib_test
{
    public class EFLifeCourseRepositoryTests
    {
        private DbContextOptions<LinklivesContext> dbContextOptions =
            new DbContextOptionsBuilder<LinklivesContext>()
            .UseInMemoryDatabase(databaseName: "LinkLivesDB")
            .Options;

        private List<LifeCourse> GetLifeCourses()
        {
            // Add a lifecourse
            var link1 = new Link() { Pa_id1 = 1, Pa_id2 = 2, Source_id1 = 1, Source_id2 = 2 };
            link1.InitKey();

            var lc1 = new LifeCourse();
            lc1.Links = new List<Link>() { link1 };
            lc1.InitKey();

            // Add a lifecourse
            var link2 = new Link() { Pa_id1 = 100, Pa_id2 = 200, Source_id1 = 100, Source_id2 = 200 };
            link2.InitKey();

            var lc2 = new LifeCourse();
            lc2.Links = new List<Link>() { link2 };
            lc2.InitKey();

            return new List<LifeCourse>() { lc1, lc2 };
        }

        [SetUp]
        public void ClearDB()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Test]
        public void DatabaseHasEntries()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                Assert.AreEqual(0, context.LifeCourses.ToList().Count());
                var lc = GetLifeCourses()[0];
                context.LifeCourses.Add(lc);
                context.SaveChanges();

                Assert.AreEqual(1, context.LifeCourses.ToList().Count());
            }
        }
        [Test]
        public void InsertItemsUpdateExistingItems_WithNewItem_AddIt()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];

                var rep = new EFLifeCourseRepository(context);
                var newItems = new List<LifeCourse>() { lc };
                rep.InsertItemsUpdateExistingItems(newItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("new_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }
        [Test]
        public void InsertItemsUpdateExistingItems_WithAnExistingItemWithNewVersion_UpdateIt()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];
                lc.Data_version = "old_version";

                context.LifeCourses.Add(lc);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context);
                var existingItems = new List<LifeCourse>() { lc };
                rep.InsertItemsUpdateExistingItems<LifeCourse>(existingItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("new_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }

        [Test]
        public void InsertItemsUpdateExistingItems_WithAnExistingItemAndANewItem_MarkExistingHistoricUpdateDataVersionAddNewItem()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lcs = GetLifeCourses();
                lcs[0].Data_version = "old_version";

                context.LifeCourses.Add(lcs[0]);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context);
                var newItems = new List<LifeCourse>() { lcs[1] };
                rep.InsertItemsUpdateExistingItems(newItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(2, dbItems.Count());

                Assert.AreEqual("old_version", dbItems[0].Data_version);
                Assert.IsTrue(dbItems[0].Is_historic);

                Assert.AreEqual("new_version", dbItems[1].Data_version);
                Assert.IsFalse(dbItems[1].Is_historic);
            }
        }
        [Test]
        public void InsertItemsUpdateExistingItems_WithExistingItemWithSameDataVersion_LeaveAsIs()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lcs = GetLifeCourses();
                lcs[0].Data_version = "existing_version";

                context.LifeCourses.Add(lcs[0]);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context);
                var newItems = new List<LifeCourse>() { lcs[0] };
                rep.InsertItemsUpdateExistingItems(newItems, "existing_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("existing_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }
    }
}
