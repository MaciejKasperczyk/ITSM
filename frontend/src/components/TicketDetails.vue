<template>
    <div class="flex-1 bg-white shadow-lg p-6 rounded-md">
      <div class="mb-6">
        <h2 v-if="ticket" class="text-2xl font-bold mb-4">
          #{{ ticket.id }} - {{ ticket.title }}
        </h2>
        <p v-else class="text-gray-500">Loading ticket details...</p>
        <p v-if="ticket" class="text-gray-700">{{ ticket.description || "No description available." }}</p>
      </div>
  
      <div v-if="ticket">
        <h2 class="text-2xl font-bold mb-4">Messages</h2>
        <div
          v-for="(message, index) in ticket.messages"
          :key="index"
          class="mb-4 p-4 bg-gray-100 border border-gray-300 rounded-md"
        >
          <p class="text-gray-600">{{ message.content }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      ticket: {
        type: Object,
        default: null, // Domyślnie `null`, co pozwala uniknąć błędu
      },
    },
  };
  </script>
  
  <style scoped>
  .bg-white {
    background-color: #ffffff;
  }
  .shadow-lg {
    box-shadow: 0 10px 15px rgba(0, 0, 0, 0.1);
  }
  </style>
  