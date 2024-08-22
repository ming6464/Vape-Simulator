using System;
using System.Collections.Generic;

namespace UnityUtils {
    public static class EnumerableExtensions {
        /// <summary>
        /// Thực hiện một hành động trên mỗi phần tử trong chuỗi.
        /// </summary>
        /// <typeparam name="T">Kiểu của các phần tử trong chuỗi.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <param name="action">Hành động để thực hiện trên mỗi phần tử.</param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action) {
            foreach (var item in sequence) {
                action(item);
            }
        }

        /// <summary>
        /// Lọc các phần tử trong chuỗi dựa trên một điều kiện.
        /// </summary>
        /// <typeparam name="T">Kiểu của các phần tử trong chuỗi.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <param name="predicate">Điều kiện để lọc các phần tử.</param>
        /// <returns>Một chuỗi các phần tử thỏa mãn điều kiện.</returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) {
            foreach (var item in sequence) {
                if (predicate(item)) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Chuyển đổi các phần tử trong chuỗi thành một dạng khác.
        /// </summary>
        /// <typeparam name="TSource">Kiểu của các phần tử trong chuỗi gốc.</typeparam>
        /// <typeparam name="TResult">Kiểu của các phần tử trong chuỗi kết quả.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <param name="selector">Hàm chuyển đổi các phần tử.</param>
        /// <returns>Một chuỗi các phần tử đã được chuyển đổi.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TResult> selector) {
            foreach (var item in sequence) {
                yield return selector(item);
            }
        }

        /// <summary>
        /// Tính toán một giá trị duy nhất từ các phần tử trong chuỗi.
        /// </summary>
        /// <typeparam name="TSource">Kiểu của các phần tử trong chuỗi.</typeparam>
        /// <typeparam name="TAccumulate">Kiểu của giá trị tích lũy.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <param name="seed">Giá trị khởi đầu của tích lũy.</param>
        /// <param name="func">Hàm để tính toán giá trị tích lũy.</param>
        /// <returns>Giá trị tích lũy cuối cùng.</returns>
        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> sequence, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) {
            TAccumulate result = seed;
            foreach (var item in sequence) {
                result = func(result, item);
            }
            return result;
        }
        
        
        
        /// <summary>
        /// Trả về phần tử đầu tiên trong chuỗi hoặc giá trị mặc định nếu chuỗi rỗng.
        /// </summary>
        /// <typeparam name="T">Kiểu của các phần tử trong chuỗi.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <returns>Phần tử đầu tiên hoặc giá trị mặc định.</returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> sequence) {
            foreach (var item in sequence) {
                return item;
            }
            return default;
        }

        /// <summary>
        /// Kiểm tra xem có bất kỳ phần tử nào trong chuỗi thỏa mãn điều kiện.
        /// </summary>
        /// <typeparam name="T">Kiểu của các phần tử trong chuỗi.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <param name="predicate">Điều kiện để kiểm tra các phần tử.</param>
        /// <returns>True nếu có bất kỳ phần tử nào thỏa mãn điều kiện, ngược lại False.</returns>
        public static bool Any<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) {
            foreach (var item in sequence) {
                if (predicate(item)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Đếm số lượng phần tử trong chuỗi thỏa mãn điều kiện.
        /// </summary>
        /// <typeparam name="T">Kiểu của các phần tử trong chuỗi.</typeparam>
        /// <param name="sequence">Chuỗi để lặp qua.</param>
        /// <param name="predicate">Điều kiện để đếm các phần tử.</param>
        /// <returns>Số lượng phần tử thỏa mãn điều kiện.</returns>
        public static int Count<T>(this IEnumerable<T> sequence, Func<T, bool> predicate) {
            int count = 0;
            foreach (var item in sequence) {
                if (predicate(item)) {
                    count++;
                }
            }
            return count;
        }
    }
}