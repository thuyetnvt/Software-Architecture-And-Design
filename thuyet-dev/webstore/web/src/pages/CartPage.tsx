import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { Trash2 } from 'lucide-react';
import { Link } from 'react-router';
import { clearCart, deleteCartItem, getCart, updateCartItem } from '../api/cart';
import { formatCurrency } from '../utils/format';

export function CartPage() {
  const queryClient = useQueryClient();
  const cartQuery = useQuery({
    queryKey: ['cart'],
    queryFn: getCart,
    retry: false
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, quantity }: { id: number; quantity: number }) => updateCartItem(id, quantity),
    onSuccess: (cart) => queryClient.setQueryData(['cart'], cart)
  });

  const deleteMutation = useMutation({
    mutationFn: deleteCartItem,
    onSuccess: (cart) => queryClient.setQueryData(['cart'], cart)
  });

  const clearMutation = useMutation({
    mutationFn: clearCart,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['cart'] })
  });

  if (cartQuery.isLoading) {
    return (
      <section className="mx-auto max-w-7xl px-4 py-8">
        <h1 className="text-2xl font-bold text-slate-950">Giỏ hàng</h1>
        <div className="mt-6 h-40 rounded-md bg-slate-100" />
      </section>
    );
  }

  if (cartQuery.isError) {
    return (
      <section className="mx-auto max-w-7xl px-4 py-8">
        <h1 className="text-2xl font-bold text-slate-950">Giỏ hàng</h1>
        <div className="mt-6 rounded-md border border-amber-200 bg-amber-50 px-4 py-5 text-sm text-amber-800">
          Bạn cần đăng nhập để xem giỏ hàng.
        </div>
      </section>
    );
  }

  const cart = cartQuery.data;

  return (
    <section className="mx-auto max-w-7xl px-4 py-8">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div>
          <h1 className="text-2xl font-bold text-slate-950">Giỏ hàng</h1>
          <p className="mt-2 text-sm text-slate-600">
            Mọi thay đổi số lượng được backend kiểm tra lại với tồn kho hiện tại.
          </p>
        </div>
        {cart?.items.length ? (
          <button
            type="button"
            onClick={() => clearMutation.mutate()}
            className="rounded-md border border-rose-300 px-3 py-2 text-sm font-medium text-rose-700 hover:bg-rose-50"
          >
            Xóa giỏ hàng
          </button>
        ) : null}
      </div>

      {!cart?.items.length ? (
        <div className="mt-6 rounded-md border border-slate-200 bg-white px-4 py-8 text-center">
          <p className="font-medium text-slate-700">Giỏ hàng đang trống.</p>
          <Link to="/products" className="mt-3 inline-block text-emerald-700 hover:underline">
            Tiếp tục mua sắm
          </Link>
        </div>
      ) : (
        <div className="mt-6 grid gap-6 lg:grid-cols-[1fr_360px]">
          <div className="space-y-3">
            {cart.items.map((item) => (
              <article key={item.id} className="rounded-md border border-slate-200 bg-white p-4">
                <div className="grid gap-4 sm:grid-cols-[96px_1fr_auto]">
                  <Link
                    to={`/products/${item.productSlug}`}
                    className="flex aspect-square items-center justify-center overflow-hidden rounded-md bg-emerald-50 text-center text-xs font-semibold text-emerald-800"
                  >
                    {item.primaryImageUrl ? (
                      <img src={item.primaryImageUrl} alt={item.productName} className="h-full w-full object-cover" />
                    ) : (
                      item.productName
                    )}
                  </Link>
                  <div>
                    <Link to={`/products/${item.productSlug}`} className="font-semibold hover:text-emerald-700">
                      {item.productName}
                    </Link>
                    <p className="mt-1 text-sm text-slate-500">SKU: {item.sku}</p>
                    <p className="mt-1 text-sm text-slate-500">
                      {[item.color, item.size].filter(Boolean).join(' • ') || 'Mặc định'}
                    </p>
                    <p className="mt-2 font-semibold text-emerald-700">{formatCurrency(item.unitPrice)}</p>
                  </div>
                  <div className="flex items-center gap-2 sm:flex-col sm:items-end">
                    <input
                      aria-label={`Số lượng ${item.productName}`}
                      type="number"
                      min={1}
                      max={item.stockQuantity}
                      value={item.quantity}
                      onChange={(event) =>
                        updateMutation.mutate({ id: item.id, quantity: Number(event.target.value) })
                      }
                      className="w-20 rounded-md border border-slate-300 px-2 py-1"
                    />
                    <p className="text-sm font-semibold">{formatCurrency(item.lineTotal)}</p>
                    <button
                      type="button"
                      onClick={() => deleteMutation.mutate(item.id)}
                      className="rounded-md p-2 text-rose-600 hover:bg-rose-50"
                      aria-label={`Xóa ${item.productName}`}
                    >
                      <Trash2 className="h-4 w-4" aria-hidden="true" />
                    </button>
                  </div>
                </div>
              </article>
            ))}
          </div>

          <aside className="h-fit rounded-md border border-slate-200 bg-white p-4">
            <h2 className="text-lg font-bold text-slate-950">Tóm tắt giỏ hàng</h2>
            <div className="mt-4 space-y-2 text-sm">
              <div className="flex justify-between">
                <span>Số lượng</span>
                <span>{cart.totalQuantity}</span>
              </div>
              <div className="flex justify-between font-semibold">
                <span>Tạm tính</span>
                <span>{formatCurrency(cart.subtotal)}</span>
              </div>
            </div>
            <Link
              to="/checkout"
              className="mt-5 block rounded-md bg-emerald-700 px-4 py-2 text-center font-semibold text-white hover:bg-emerald-800"
            >
              Đi tới thanh toán
            </Link>
          </aside>
        </div>
      )}
    </section>
  );
}
