/*
 * This code is provided as is with no warranty. If you find a bug please report it on github.
 * If you would like to use the code please leave this comment at the top of the page
 * License MIT (c) Brent McKendrick 2012
 */

using System;
using System.Data.Entity;
using System.Linq.Expressions;
using RefactorThis.GraphDiff.Internal;
using RefactorThis.GraphDiff.Internal.Graph;

namespace RefactorThis.GraphDiff
{
    public static class DbContextExtensions
    {
        /// <summary>
        ///     Merges a graph of entities with the data store.
        /// </summary>
        /// <typeparam name="T">The type of the root entity</typeparam>
        /// <param name="context">The database context to attach / detach.</param>
        /// <param name="entity">The root entity.</param>
        /// <param name="mapping">The mapping configuration to define the bounds of the graph</param>
        /// <param name="allowDelete">Allow to delete from graph.</param>
        /// <returns>The attached entity graph</returns>
        public static T UpdateGraph<T>(this DbContext context, T entity,
            Expression<Func<IUpdateConfiguration<T>, object>> mapping = null, bool allowDelete = true)
            where T : class, new()
        {
            var root = mapping == null ? new GraphNode() : new ConfigurationVisitor<T>().GetNodes(mapping);
            root.AllowDelete = allowDelete;
            var graphDiffer = new GraphDiffer<T>(root);
            return graphDiffer.Merge(context, entity);
        }

        /// <summary>
        ///     Merges a graph of entities with the data store.
        /// </summary>
        /// <typeparam name="T">The type of the root entity</typeparam>
        /// <param name="context">The database context to attach / detach.</param>
        /// <param name="entity">The root entity.</param>
        /// <param name="allowDelete">Allow to delete from graph.</param>
        /// <returns>The attached entity graph</returns>
        public static T UpdateGraph<T>(this DbContext context, T entity, bool allowDelete)
            where T : class, new()
        {
            return context.UpdateGraph(entity, null, allowDelete);
        }

        // TODO add IEnumerable<T> entities
    }
}
