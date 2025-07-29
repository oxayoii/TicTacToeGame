<template>
  <div>
    <button @click="createGame" :disabled="loading">
      {{ loading ? 'Создание игры...' : 'Создать новую игру' }}
    </button>
    <p v-if="error" style="color: red;">Ошибка: {{ error }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      loading: false,
      error: null,
    };
  },
  methods: {
    async createGame() {
      this.loading = true;
      this.error = null;

      try {
        const response = await axios.post('http://localhost:5009/Game'); 
        this.$emit('game-created', response.data); 
      } catch (err) {
        this.error = err.response?.data?.error || 'Не удалось создать игру.'; // Изменено для получения сообщения из JSON
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>