using Hub.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Hub.DataAccess.Repositories
{
    public static class SectionExtensions
    {
        public static void TransformSectionToContentSection(this EntityRepository<Section> testRepo, int nodeId)
        {
            string sqlQuery = "UPDATE Sections SET Discriminator ='ContentSection' WHERE ID=" + nodeId;
            testRepo.RunSqlQuery(sqlQuery);
        }

        /// <summary>
        /// Loads section with it's children
        /// </summary>
        /// <param name="testRepo"></param>
        /// <param name="rootId"></param>
        /// <returns></returns>
        public static Section LoadChildSectionsWithParent(this EntityRepository<Section> testRepo, int parentId)
        {
            var aSections = from section in testRepo.GetAllQueryable()
                           where section.ParentSectionID == parentId
                            select new
                           {
                               ID = section.ID,
                               Name = section.Name,
                               ParentSection = section.ParentSection,
                               ParentSectionID = section.ParentSectionID,
                               ChildrenCount = section.Sections.Count(),
                               IsContent = section is ContentSection
                           };

            if (aSections.Count() > 0)
            {
                var sections = aSections.AsEnumerable().Select(res => res.IsContent ? res.ToType<ContentSection>() : res.ToType<Section>()).Cast<Section>().ToList();
                sections.ElementAt(0).ParentSection.Sections = sections.ToList();
                sections.ElementAt(0).ParentSection.ChildrenCount = sections.Count();
                return sections.ElementAt(0).ParentSection;
            }

            return null;
        }

        /// <summary>
        /// Loads child sections of a section
        /// Loading with null will return the root section(s)
        /// </summary>
        /// <param name="testRepo"></param>
        /// <returns></returns>
        public static IEnumerable<Section> LoadChildSectionsForParent(this EntityRepository<Section> testRepo, int? parentId)
        {
            var aSections = from section in testRepo.GetAllQueryable()
                            where section.ParentSectionID == parentId
                            select new
                            {
                                ID = section.ID,
                                Name = section.Name,
                                ParentSectionID = section.ParentSectionID,
                                ChildrenCount = section.Sections.Count(),
                                IsContent = section is ContentSection
                            };

            if (aSections.Count() > 0)
            {
                var sections = aSections.AsEnumerable().Select(res => res.IsContent ? res.ToType<ContentSection>() : res.ToType<Section>()).Cast<Section>().ToList();
                return sections;
            }

            return null;
        }
    }
}
