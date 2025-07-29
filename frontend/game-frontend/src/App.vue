<template>
  <div id="app">
    <h1>Крестики-Нолики</h1>
    <NewGame v-if="gameId === null" @game-created="handleGameCreated" />
    <Game v-else :gameId="gameId" @new-game-created="handleNewGameCreated"/>
  </div>
</template>

<script>
import NewGame from './components/NewGame.vue';
import Game from './components/Game.vue';

export default {
  components: {
    NewGame,
    Game,
  },
  data() {
    return {
      gameId: localStorage.getItem('gameId') || null,
    };
  },
  methods: {
    handleGameCreated(newGameId) {
      this.gameId = newGameId;
      localStorage.setItem('gameId', newGameId);
    },
    handleNewGameCreated() {
      this.gameId = null; // Обнуляем gameId
      localStorage.removeItem('gameId'); // Удаляем из localStorage
    }
  },
};
</script>

<style>
#app {
  background-color:beige;
  font-family: Avenir, Helvetica, Arial, sans-serif;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
