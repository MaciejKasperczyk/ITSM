<template>
    <div class="flex flex-wrap gap-4 mb-6">
      <div>
        <label for="status-filter" class="block text-sm font-medium mb-1">Status:</label>
        <select
          id="status-filter"
          v-model="filters.status"
          class="w-full p-2 border border-gray-300 rounded-md"
          @change="$emit('filter-changed', filters)"
        >
          <option value="">All</option>
          <option value="Open">Open</option>
          <option value="In Progress">In Progress</option>
          <option value="Resolved">Resolved</option>
          <option value="Closed">Closed</option>
        </select>
      </div>
      <div>
        <label for="priority-filter" class="block text-sm font-medium mb-1">Priority:</label>
        <select
          id="priority-filter"
          v-model="filters.priority"
          class="w-full p-2 border border-gray-300 rounded-md"
          @change="$emit('filter-changed', filters)"
        >
          <option value="">All</option>
          <option value="Low">Low</option>
          <option value="Medium">Medium</option>
          <option value="High">High</option>
          <option value="Critical">Critical</option>
        </select>
      </div>
      <div>
        <label for="assignee-filter" class="block text-sm font-medium mb-1">Assigned IT:</label>
        <select
          id="assignee-filter"
          v-model="filters.assignedIt"
          class="w-full p-2 border border-gray-300 rounded-md"
          @change="$emit('filter-changed', filters)"
        >
          <option value="">All</option>
          <option
            v-for="user in users"
            :key="user.id"
            :value="user.fullName"
          >
            {{ user.fullName }}
          </option>
        </select>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      users: {
        type: Array,
        required: true,
      },
      initialFilters: {
        type: Object,
        default: () => ({
          status: "",
          priority: "",
          assignedIt: "",
        }),
      },
    },
    data() {
      return {
        filters: { ...this.initialFilters },
      };
    },
  };
  </script>
  
  <style scoped>
  /* Stylowanie komponentu filtrowania */
  select {
    width: 100%;
    padding: 8px;
    border-radius: 6px;
    border: 1px solid #ddd;
  }
  </style>
  