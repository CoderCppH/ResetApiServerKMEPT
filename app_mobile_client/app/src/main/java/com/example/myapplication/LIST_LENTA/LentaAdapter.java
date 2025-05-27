package com.example.myapplication.LIST_LENTA;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.R;

import java.util.List;

public class LentaAdapter extends RecyclerView.Adapter<LentaAdapter.ViewHolder> {
    private List<p_lenta_item> list_lenta;
    private LayoutInflater lInFlater;
    private OnItemClickListener listener;

    public interface OnItemClickListener {
        void onItemClick(int position);
    }

    public void setOnItemClickListener(OnItemClickListener listener) {
        this.listener = listener;
    }

    public LentaAdapter(LayoutInflater inflater, List<p_lenta_item> list_user) {
        this.lInFlater = inflater;
        this.list_lenta = list_user;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = this.lInFlater.inflate(R.layout.my_custom_item_list_lenta, parent, false);
        return new ViewHolder(view, listener);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        p_lenta_item item = list_lenta.get(position);

        try {
            byte[] decodedBytes = Base64.decode(item.getImage_post(), Base64.DEFAULT);
            Bitmap bitmap = BitmapFactory.decodeByteArray(decodedBytes, 0, decodedBytes.length);
            holder.image.setImageBitmap(bitmap);
        } catch (Exception e) {
            e.printStackTrace();
        }

        holder.name.setText(item.getName_post());
        holder.authorEmail.setText(item.getEmail());

    }

    @Override
    public int getItemCount() {
        return list_lenta.size();
    }

    public void AddItem(p_lenta_item item) {
        list_lenta.add(item);
        notifyItemInserted(list_lenta.size() - 1);
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        TextView name;
        ImageView image;
        TextView authorEmail; // Добавлено поле для email автора

        public ViewHolder(@NonNull View itemView, final OnItemClickListener listener) {
            super(itemView);
            name = itemView.findViewById(R.id.id_item_lenta_name);
            image = itemView.findViewById(R.id.id_item_lenta_image);
            authorEmail = itemView.findViewById(R.id.id_item_lenta_author_email); // Инициализация нового поля

            if (listener != null) {
                itemView.setOnClickListener(v -> {
                    int position = getAdapterPosition();
                    if (position != RecyclerView.NO_POSITION) {
                        listener.onItemClick(position);
                    }
                });
            }
        }
    }
}