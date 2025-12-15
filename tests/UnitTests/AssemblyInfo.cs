using Xunit;
/* this is needed to tell xunit to run each [Fact] sequentially on a single thread
 * if I do not do this multithreading causes tests accessing the activegames dictionarry to fail
 * I believe this is better than using a concurrentDictionary just for test */
[assembly: CollectionBehavior(DisableTestParallelization = true)]