﻿using Linklives.DAL;
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
            new DbContextOptionsBuilder<LinklivesContext>().UseMySQL(@"server=127.0.0.1;uid=root;pwd=123456;database=linklives_data_test").Options;

        private List<LifeCourse> GetLifeCourses()
        {
            var links = GetLinks();

            // Add a lifecourse
            var lc1 = new LifeCourse();
            lc1.Links = new List<Link>() { links[0] };
            lc1.InitKey();

            // Add a lifecourse
            var lc2 = new LifeCourse();
            lc2.Links = new List<Link>() { links[1] };
            lc2.InitKey();

            return new List<LifeCourse>() { lc1, lc2 };
        }

        private List<Link> GetLinks()
        {
            // Add a link
            var link1 = new Link() { Pa_id1 = 1, Pa_id2 = 2, Source_id1 = 1, Source_id2 = 2 };
            link1.InitKey();

            // Add a link
            var link2 = new Link() { Pa_id1 = 10, Pa_id2 = 20, Source_id1 = 10, Source_id2 = 20 };
            link2.InitKey();

            // Add a link
            var link3 = new Link() { Pa_id1 = 100, Pa_id2 = 200, Source_id1 = 100, Source_id2 = 200 };
            link3.InitKey();

            return new List<Link>() { link1, link2, link3 };
        }

        [SetUp]
        public void ClearDB()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                if (!context.Database.EnsureDeleted())
                {
                    throw new System.Exception("Could not clear the database. Tests are not run.");
                }
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
        public void AddNewItems_WithNewItem_AddItemWithDataVersion()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];

                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var newItems = new List<LifeCourse>() { lc };
                rep.Upsert(newItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("new_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }

        [Test]
        public void AddNewItems_WithNewItemWithNewAndExistingLinks_AddItemAndNewLinks()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];
                context.LifeCourses.Add(lc);
                context.SaveChanges();
                context.ChangeTracker.Clear();

                var newLc = GetLifeCourses()[1];
                var existingLink = GetLinks()[0];
                var newLink = GetLinks()[2];
                newLc.Links.Clear();
                newLc.Links.Add(existingLink);
                newLc.Links.Add(newLink);
                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var newItems = new List<LifeCourse>() { newLc };
                rep.Upsert(newItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(2, dbItems.Count());
                Assert.AreEqual(2, dbItems[1].Links.Count());
            }
        }

        [Test]
        public void AddNewItems_WithNewItemWithExistingLinks_AddItemIgnoreLinks()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];
                context.LifeCourses.Add(lc);
                context.SaveChanges();

                var newLcExistingLinks = GetLifeCourses()[1];
                newLcExistingLinks.Links.Clear();
                newLcExistingLinks.Links.Add(lc.Links.First());
                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var newItems = new List<LifeCourse>() { newLcExistingLinks };
                rep.Upsert(newItems, "new_version");

                context.LifeCourses.Load();
                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(2, dbItems.Count());
                Assert.AreEqual(1, dbItems[1].Links.Count());
            }
        }

        [Test]
        public void AddNewItems_WithExistingItem_IgnoreIt()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];
                context.LifeCourses.Add(lc);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var newItems = new List<LifeCourse>() { lc };
                rep.Upsert(newItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual(1, dbItems[0].Links.Count());
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }

        [Test]
        public void AddNewItems_WithExistingItemWithNewVersion_UpdateIt()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lc = GetLifeCourses()[0];
                lc.Data_version = "old_version";
                context.LifeCourses.Add(lc);
                context.SaveChanges();
                context.ChangeTracker.Clear();

                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var existingItems = new List<LifeCourse>() { lc };
                rep.Upsert(existingItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("new_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }

        [Test]
        public void AddNewItems_WithExistingItemWithSameDataVersion_UpdateIt()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lcs = GetLifeCourses();
                lcs[0].Data_version = "existing_version";
                context.LifeCourses.Add(lcs[0]);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var newItems = new List<LifeCourse>() { lcs[0] };
                rep.Upsert(newItems, "new_version");

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("new_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }

        [Test]
        public void MarkOldItems_WithNewItems_UpdateExistingToIsHistoric()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lcs = GetLifeCourses();
                lcs[0].Data_version = "old_version";

                context.LifeCourses.Add(lcs[0]);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var items = new List<LifeCourse>() { lcs[1] };
                rep.MarkOldItems(items);

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("old_version", dbItems[0].Data_version);
                Assert.IsTrue(dbItems[0].Is_historic);
            }
        }

        [Test]
        public void MarkOldItems_WithExistingItems_LeaveAsIs()
        {
            using (var context = new LinklivesContext(dbContextOptions))
            {
                var lcs = GetLifeCourses();
                lcs[0].Data_version = "old_version";

                context.LifeCourses.Add(lcs[0]);
                context.SaveChanges();

                var rep = new EFLifeCourseRepository(context, dbContextOptions);
                var items = new List<LifeCourse>() { lcs[0] };
                rep.MarkOldItems(items);

                var dbItems = context.LifeCourses.ToList();
                Assert.AreEqual(1, dbItems.Count());
                Assert.AreEqual("old_version", dbItems[0].Data_version);
                Assert.IsFalse(dbItems[0].Is_historic);
            }
        }
    }
}
