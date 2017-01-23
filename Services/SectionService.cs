using Hub.DataAccess.Repositories;
using Hub.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Hub.Services
{
    public class SectionService
    {
        private EntityRepository<Section> _sectionRepository;

        //
        //change this with IoC later
        public SectionService()
        {
            _sectionRepository = new EntityRepository<Section>(new DataAccess.HubDbContext());
        }

        public Section GetEntity(int sectionID)
        {
            return _sectionRepository.GetEntity(sectionID);
        }


        public void TransformSectionToContentSection(int nodeId)
        {
            _sectionRepository.TransformSectionToContentSection(nodeId);
            //refresh the context!
            _sectionRepository = new EntityRepository<Section>(new DataAccess.HubDbContext());
        }

        public void RenameSection(int nodeId, string newName)
        {
            var s = GetEntity(nodeId);
            if (s != null)
                s.Name = newName;
            _sectionRepository.CommitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns>A tuple with element 1: parent of the deleted section and element 2 a boolean indicating if the section had children</returns>
        public void DeleteSectionWithoutChildren(int sectionID)
        {
            var entity = GetEntity(sectionID);
            int? newParentId = entity.ParentSectionID;

            var children = _sectionRepository.GetAll().Where(sec => sec.ParentSectionID == sectionID).ToList();

            if (children.Count() != 0)
            {
                foreach (var child in children)
                    child.ParentSectionID = entity.ParentSectionID;
            }

            _sectionRepository.DeleteOnCommit(entity);
            _sectionRepository.CommitChanges();
        }

        public Section CreateEmptySection(int? parentSectionId)
        {
            Section s = new Section();
            s.Name = "NEW";
            s.ParentSectionID = parentSectionId;
            _sectionRepository.InsertOnCommit(s);
            _sectionRepository.CommitChanges();

            return s;
        }

        public void SaveChanges()
        {
            _sectionRepository.CommitChanges();
        }

        public IEnumerable<Section> LoadChildSectionsForParent(int? parentId)
        {
            var children = _sectionRepository.LoadChildSectionsForParent(parentId);

            if (children != null)
                children.OrderBy(s => s.ID);

            return children;
        }

        public Section LoadChildSectionsWithParent(int parentId)
        {
            var sec = _sectionRepository.LoadChildSectionsWithParent(parentId);

            //no children for this parent
            if (sec == null)
                return _sectionRepository.GetEntity(parentId);

            sec.Sections.OrderBy(s => s.ID);

            return sec;
        }

    }
}