<template>
    <div
      v-if="ticket"
      class="bg-gray-100 border border-gray-300 rounded-lg p-6 shadow text-center self-start w-64"
    >
      <h3 class="text-lg font-semibold mb-4">Ticket Information</h3>
      <div class="space-y-4 text-left">
        <div>
          <label class="block font-semibold mb-1">Created by:</label>
          <select
            v-model="localTicket.userId"
            class="w-full p-2 border border-gray-300 rounded-md"
          >
            <option
              v-for="user in users"
              :key="user.id"
              :value="user.id"
            >
              {{ user.fullName }}
            </option>
          </select>
        </div>
  
        <div>
          <label class="block font-semibold mb-1">Assigned IT:</label>
          <select
            v-model="localTicket.itUserId"
            class="w-full p-2 border border-gray-300 rounded-md"
          >
            <option value="">Unassigned</option>
            <option
              v-for="user in users"
              :key="user.id"
              :value="user.id"
            >
              {{ user.fullName }}
            </option>
          </select>
        </div>
  
        <div>
          <label class="block font-semibold mb-1">Status:</label>
          <select
            v-model="localTicket.status"
            class="w-full p-2 border border-gray-300 rounded-md"
          >
            <option value="Open">Open</option>
            <option value="In Progress">In Progress</option>
            <option value="Resolved">Resolved</option>
            <option value="Closed">Closed</option>
          </select>
        </div>
  
        <div>
          <label class="block font-semibold mb-1">Priority:</label>
          <select
            v-model="localTicket.priority"
            class="w-full p-2 border border-gray-300 rounded-md"
          >
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
            <option value="Critical">Critical</option>
          </select>
        </div>
  
        <div>
          <label class="block font-semibold mb-1">Type:</label>
          <select
            v-model="localTicket.type"
            class="w-full p-2 border border-gray-300 rounded-md"
          >
            <option :value="0">Incident</option>
            <option :value="1">Service Request</option>
          </select>
        </div>
  
        <button
          @click="applyChanges"
          class="w-full mt-4 px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
        >
          Apply Changes
        </button>
      </div>
    </div>
  
    <p v-else class="text-gray-500">Loading ticket information...</p>
  </template>
  
  <script>
  export default {
    props: {
      ticket: {
        type: Object,
        default: null,
      },
      users: {
        type: Array,
        required: true,
      },
    },
    data() {
      return {
        localTicket: {},
      };
    },
    watch: {
      ticket: {
        immediate: true,
        handler(newValue) {
          if (newValue) {
            this.localTicket = JSON.parse(JSON.stringify(newValue));
          }
        },
      },
    },
    methods: {
      applyChanges() {
        this.$emit("applyChanges", this.localTicket);
      },
    },
  };
  </script>
  