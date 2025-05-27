package com.example.myapplication.UI.LentaListFragment;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.myapplication.ACTIVITY.AddLentaViewActivity;
import com.example.myapplication.ACTIVITY.LentaViewActivity;
import com.example.myapplication.API.HttpClient;
import com.example.myapplication.API.json_p_lenta;
import com.example.myapplication.API.json_p_user;
import com.example.myapplication.GL.GL;
import com.example.myapplication.LIST_LENTA.LentaAdapter;
import com.example.myapplication.LIST_LENTA.p_lenta_item;
import com.example.myapplication.R;
import com.google.android.material.appbar.MaterialToolbar;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.CompletableFuture;

public class LentaListFragment extends Fragment {

    private RecyclerView recyclerView;
    private LentaAdapter adapter;
    private List<p_lenta_item> list = new ArrayList<>();
    private MaterialToolbar toolbar;
    private HttpClient api = new HttpClient();
    private Gson gson = new Gson();

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_lenta_list, container, false);

        initViews(view);
        setupRecyclerView(inflater);
        setupListeners();
        refreshData();

        return view;
    }

    private void initViews(View view) {
        toolbar = view.findViewById(R.id.btn_refresh);
        recyclerView = view.findViewById(R.id.recycler_view);
        view.findViewById(R.id.btn_add_post).setOnClickListener(v ->
                startActivity(new Intent(getContext(), AddLentaViewActivity.class))
        );
    }

    private void setupRecyclerView(LayoutInflater inflater) {
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        adapter = new LentaAdapter(inflater, list);
        recyclerView.setAdapter(adapter);

        adapter.setOnItemClickListener(position -> {
            p_lenta_item clickedItem = list.get(position);
            Intent intent = new Intent(getContext(), LentaViewActivity.class);
            intent.putExtra("lenta_json", gson.toJson(clickedItem));
            startActivity(intent);
        });
    }

    private void setupListeners() {
        toolbar.setOnClickListener(v -> refreshData());
    }

    private void refreshData() {
        CompletableFuture.runAsync(() -> {
            try {
                String response = api.GET(GL.url_api_server + "lenta");
                Type lentaListType = new TypeToken<List<json_p_lenta>>() {}.getType();
                List<json_p_lenta> lentaList = gson.fromJson(response, lentaListType);

                List<p_lenta_item> newList = new ArrayList<>();
                for (json_p_lenta lenta : lentaList) {
                    json_p_user user = gson.fromJson(
                            api.GET(GL.url_api_server + "users/" + lenta.id_user + "/"),
                            json_p_user.class
                    );
                    newList.add(new p_lenta_item(
                            lenta.id,
                            lenta.name_post,
                            lenta.description_post,
                            lenta.image_post,
                            lenta.id_user,
                            user.email
                    ));
                }

                requireActivity().runOnUiThread(() -> {
                    list.clear();
                    list.addAll(newList);
                    adapter.notifyDataSetChanged();
                });
            } catch (Exception e) {
                Log.e("API", "Ошибка загрузки: " + e.getMessage());
                requireActivity().runOnUiThread(() ->
                        Toast.makeText(getContext(), "Ошибка обновления", Toast.LENGTH_SHORT).show()
                );
            }
        });
    }

    @Override
    public void onResume() {
        super.onResume();
        refreshData();
    }
}