using System.Collections;
using System.Collections.Generic;
using System;

namespace Utility {
	public static class Extensions {

		private static Random rng = new Random();  

		public static void Shuffle<T>(this IList<T> list) {  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = rng.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}  
		}

        public static T GetAt<T>(this LinkedList<T> list, int index) {
            if(list.Count <= index) {
                int i = 0;
                foreach(T item in list) {
                    if (index == i)
                        return item;
                    i++;
                }
            }
            return default(T);
        }

}
}