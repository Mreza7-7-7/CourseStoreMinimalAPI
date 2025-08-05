namespace CourseStoreMinimalAPI.Entities
{
    public sealed class Category : BaseEntities
    {
        public string Name { get; set; }
        public List<CourseCategory> courseCategories { get; set; }
    }
}
