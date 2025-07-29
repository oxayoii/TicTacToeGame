<template>
  <div class="container">
    <div class="game-panel">
      <h2 v-if="game">Игра #{{ game.id }}</h2>
      <p v-if="game">Текущий игрок: {{ game.currentPlayerSymbol }}</p>

      <div v-if="game" class="board">
        <div
          v-for="(cell, index) in game.boardState"
          :key="index"
          class="cell"
          :class="{ selected: selectedCell && selectedCell.row === getRow(index) && selectedCell.col === getCol(index) }"
          @click="handleCellClick(getRow(index), getCol(index))"
        >
          {{ cell || '' }}
        </div>
      </div>

      <button @click="makeMove" :disabled="loading || !selectedCell || !gameId" class="action-button">
        {{ loading ? 'Делаем ход...' : 'Сделать ход' }}
      </button>
      <p v-if="game && game.isCompleted">
        Игра завершена!
        <span v-if="game.winnerSymbol">Победил игрок {{ game.winnerSymbol }}</span>
        <span v-else>Ничья</span>
      </p>

      <p v-if="error" style="color: red;">{{ error }}</p> <!-- Вывод ошибки -->
      <p v-if="loading">Загрузка...</p>

      <button @click="startNewGame" :disabled="loading" class="action-button">Начать новую игру</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  props: {
    gameId: {
      type: Number, 
      required: true,
    },
  },
  data() {
    return {
      game: null,
      error: null,
      loading: false,
      selectedCell: null,
      eTag: null,
    };
  },
  mounted() {
    if (this.gameId) {
      this.fetchGame();
    } else {
      console.warn('Game.vue - mounted without gameId');
    }
  },
  methods: {
    async fetchGame() {
      this.loading = true;
      this.error = null;

      try {
        const response = await axios.get(`http://localhost:5009/Game/${this.gameId}`);
        this.game = response.data;
        this.eTag = response.headers['etag'];
      } catch (err) {
        this.error = err.response?.data?.error || 'Не удалось загрузить игру.'; // Изменено для получения сообщения из JSON
      } finally {
        this.loading = false;
      }
    },
    getRow(index) {
      return Math.floor(index / this.game.boardSize);
    },
    getCol(index) {
      return index % this.game.boardSize;
    },
    handleCellClick(row, col) {
      this.selectedCell = { row, col };
    },
    async makeMove() {
      if (!this.selectedCell) {
        alert('Выберите ячейку для хода.');
        return;
      }

      this.loading = true;
      this.error = null;

      try {
        const response = await axios.post(
          `http://localhost:5009/Game/${this.gameId}/moves`,
          { row: this.selectedCell.row, column: this.selectedCell.col },
          {
            headers: {
              'If-Match': this.eTag,
            },
          }
        );

        this.game = response.data;
        this.eTag = response.headers['etag'];
      } catch (err) {
        this.error = err.response?.data?.error || 'Не удалось сделать ход.'; // Изменено для получения сообщения из JSON
      } finally {
        this.loading = false;
        this.selectedCell = null;
      }
    },
    startNewGame() {
      this.$emit('new-game-created');
    },
  },
};
</script>

<style scoped>
.container {
  display: flex;
  justify-content: center; /* Центрирование по горизонтали */
  height: 100vh; /* Высота на весь экран */
}

.game-panel {
  display: flex;
  flex-direction: column;
  align-items: center; /* Центрирование содержимого внутри панели */
}

.board {
  display: grid;
  grid-template-columns: repeat(3, 100px); /* Увеличен размер ячеек */
  grid-gap: 5px; /* Увеличен отступ между ячейками */
  margin-bottom: 10px;
}

.cell {
  background-color: white;
  width: 100px; /* Увеличен размер ячейки */
  height: 100px; /* Увеличен размер ячейки */
  border: 1px solid #ccc;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 30px; /* Увеличен размер шрифта */
  cursor: pointer;
}

.cell.selected {
  background-color: lightblue;
}

.action-button {
  margin-top: 5px; /* Добавляет отступ между кнопками */
}
</style>