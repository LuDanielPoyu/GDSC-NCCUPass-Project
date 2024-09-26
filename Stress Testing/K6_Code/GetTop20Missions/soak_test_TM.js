import http from 'k6/http';
import { sleep } from 'k6';
import { check } from 'k6';

const token = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6IjExMDMwNjAxOUBuY2N1LmVkdS50dyIsIlJvbGUiOiJOQ0NVU3R1ZGVudCIsIlVzZXJJZCI6IjYzODViMTcwNGI3OWM0MTZkNzZlZDQ4OCIsImV4cCI6MTcyNTcyOTQxNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI0MyIsImF1ZCI6ImZyb250LWVuZC11cmwifQ.7-CD_uqKGQSo-O4i_533I5qaBkPHuI1BgIQKnUIO_lA";
const url_get20missions = "https://localhost:7243/nccupass/NormalTask/mainPage/20/0";

export const options = {
  stages: [
    { duration: '1m', target: 100 }, // traffic ramp-up from 1 to 100 users over 5 minutes.
    { duration: '30m', target: 100 }, // stay at 100 users for 8 hours!!!
    // { duration: '1.6h', target: 100 }, // stay at 100 users for 8 hours!!!
    { duration: '1m', target: 0 }, // ramp-down to 0 users
  ],
};

export default function () {
  const headers = {
    'accept': 'text/plain', // Update the accept header to 'application/json'
    'Authorization': token,
  };

  // 1st vers (X: stop after 20 min, timeout request)
  // const res = http.get(url_get20missions, { headers, timeout: '60s' });
  // 2nd vers (X: stop after 13 min, timeout request)
  // const res = http.get(url_get20missions, { headers, timeout: '120s' });
  // 3rd vers (still haven't tried)
  const res = http.get(url_get20missions, {headers});


  check(res, {
    'Status is 200': (r) => r.status === 200,
  });

  sleep(1);
}