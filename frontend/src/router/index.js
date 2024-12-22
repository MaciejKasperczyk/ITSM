import { createRouter, createWebHistory } from "vue-router";
import TicketsList from "../components/TicketsList.vue";
import TicketDashboard from "../components/TicketDashboard.vue";
import Login from "../components/Login.vue";
import NewTicket from "../components/NewTicket.vue";

const routes = [
  { path: "/", component: TicketsList, meta: { requiresAuth: true } },
  { path: "/tickets/new", component: NewTicket, meta: { requiresAuth: true } },
  { path: "/tickets/:id/details", component: TicketDashboard, meta: { requiresAuth: true } },
  { path: "/login", component: Login },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  if (to.meta.requiresAuth && !token) {
    next("/login");
  } else {
    next();
  }
});

export default router;
