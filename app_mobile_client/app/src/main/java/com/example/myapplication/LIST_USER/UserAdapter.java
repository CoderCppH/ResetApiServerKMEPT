package com.example.myapplication.LIST_USER;

import android.content.Context;
import android.content.IntentFilter;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.LIST_LENTA.p_lenta_item;
import com.example.myapplication.R;

import java.util.List;

public class UserAdapter extends RecyclerView.Adapter<UserAdapter.ViewHolder> {
    private List<p_user_item> list_user;
    private LayoutInflater lInFlater;
    public  UserAdapter(LayoutInflater inflater, List<p_user_item> list_user) {
        this.lInFlater = inflater;
        this.list_user = list_user;
    }

    @NonNull
    @Override
    public UserAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = this.lInFlater.inflate(R.layout.my_custom_item_list_user, parent, false);


        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull UserAdapter.ViewHolder holder, int position) {
        p_user_item item = list_user.get(position);
        holder.img_profile.setImageResource(item.getImage());
        holder.fullname.setText(item.getFullName());
        holder.email.setText(item.getEmail());
    }

    @Override
    public int getItemCount() {
        return list_user.size();
    }
    public void AddItem(p_user_item item) {
        list_user.add(item);
        notifyItemInserted(list_user.size() - 1);
    }
    public class ViewHolder extends RecyclerView.ViewHolder
    {
        TextView fullname, email;
        ImageView img_profile;
        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            fullname = itemView.findViewById(R.id.id_const_item_user_list_full_name);
            email = itemView.findViewById(R.id.id_const_item_user_list_email);
            img_profile = itemView.findViewById(R.id.id_const_item_user_list_image_profile);
        }
    }
}
