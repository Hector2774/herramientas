import math, sys
from xml.sax.saxutils import escape

W, H = 1900, 1420
AZUL, CLARO, GRIS, LINEA = "#1F4E78", "#DDEBF7", "#F2F2F2", "#404040"
out = []
lin = []
lab = []


def text(x, y, lines, size=13, color="#000", weight="normal", anchor="middle"):
    lh = size * 1.25
    y0 = y - lh * (len(lines) - 1) / 2 + size * 0.35
    t = "".join(f'<tspan x="{x}" y="{y0 + i * lh:.1f}">{escape(l)}</tspan>' for i, l in enumerate(lines))
    out.append(f'<text font-family="Arial" font-size="{size}" fill="{color}" font-weight="{weight}" text-anchor="{anchor}">{t}</text>')


class Rect:
    def __init__(s, cx, cy, w, h):
        s.cx, s.cy, s.w, s.h = cx, cy, w, h

    def edge(s, dx, dy):
        tx = (s.w / 2) / abs(dx) if dx else 1e9
        ty = (s.h / 2) / abs(dy) if dy else 1e9
        t = min(tx, ty)
        return s.cx + dx * t, s.cy + dy * t


class Circ:
    def __init__(s, cx, cy, r):
        s.cx, s.cy, s.r = cx, cy, r

    def edge(s, dx, dy):
        n = math.hypot(dx, dy)
        return s.cx + dx / n * s.r, s.cy + dy / n * s.r


def entidad(cx, cy, w, h, lines, dup=False):
    x, y = cx - w / 2, cy - h / 2
    out.append(f'<rect x="{x}" y="{y}" width="{w}" height="{h}" fill="{CLARO}" stroke="{AZUL}" stroke-width="2"/>')
    if dup:  # entidad duplicada: línea diagonal en la esquina inferior derecha
        out.append(f'<line x1="{x + w - 18}" y1="{y + h}" x2="{x + w}" y2="{y + h - 18}" stroke="{AZUL}" stroke-width="2"/>')
    text(cx, cy, lines, 15)
    return Rect(cx, cy, w, h)


def proceso(cx, cy, num, lines, r=64):
    out.append(f'<circle cx="{cx}" cy="{cy}" r="{r}" fill="{AZUL}"/>')
    text(cx, cy, [num] + lines, 13, "#fff", "bold")
    return Circ(cx, cy, r)


def almacen(cx, cy, did, nombre, w=250, h=42, dup=False):
    x, y = cx - w / 2, cy - h / 2
    out.append(f'<rect x="{x}" y="{y}" width="{w}" height="{h}" fill="{GRIS}" stroke="none"/>')
    out.append(f'<path d="M{x + w},{y} H{x} V{y + h} H{x + w}" fill="none" stroke="#595959" stroke-width="2"/>')
    out.append(f'<line x1="{x + 44}" y1="{y}" x2="{x + 44}" y2="{y + h}" stroke="#595959" stroke-width="2"/>')
    if dup:  # almacén duplicado: línea vertical extra junto al identificador
        out.append(f'<line x1="{x + 8}" y1="{y}" x2="{x + 8}" y2="{y + h}" stroke="#595959" stroke-width="2"/>')
    text(x + 26 if dup else x + 22, cy, [did], 13, weight="bold")
    text(x + 44 + (w - 44) / 2, cy, [nombre], 13)
    return Rect(cx, cy, w, h)


def flujo(a, b, lines, off=0, lpos=0.5, lside=1, ldist=0, force=False):
    if off and not force:
        lside = 1 if off > 0 else -1
    dx, dy = b.cx - a.cx, b.cy - a.cy
    n = math.hypot(dx, dy)
    px, py = -dy / n * off, dx / n * off
    # punto de salida/llegada desplazado en paralelo
    ax, ay = a.edge(dx + px * 0, dy)  # dirección
    sx, sy = a.edge(dx, dy)
    ex, ey = b.edge(-dx, -dy)
    sx, sy, ex, ey = sx + px, sy + py, ex + px, ey + py
    lin.append(f'<line x1="{sx:.1f}" y1="{sy:.1f}" x2="{ex:.1f}" y2="{ey:.1f}" stroke="{LINEA}" stroke-width="1.6" marker-end="url(#f)"/>')
    mx, my = sx + (ex - sx) * lpos, sy + (ey - sy) * lpos
    nx, ny = -dy / n * lside, dx / n * lside
    lh = 12 * 1.25 * len(lines)
    d = (ldist or (lh / 2 + 6))
    mx, my = mx + nx * d, my + ny * d
    wmax = max(len(l) for l in lines) * 6.4 + 8
    lab.append(f'<rect x="{mx - wmax / 2:.1f}" y="{my - lh / 2 - 2:.1f}" width="{wmax:.1f}" height="{lh + 4:.1f}" fill="#fff" opacity="0.92"/>')
    n0 = len(out); text(mx, my, lines, 12, "#222"); lab.append(out.pop())


# ---------------- elementos
AD = entidad(120, 80, 180, 64, ["Administrador"])
OP_ = entidad(120, 810, 180, 1140, ["Operador", "(encargado", "de bodega)"])
OPat = lambda p: Rect(120, p.cy, 180, 10)

P1 = proceso(500, 230, "1.0", ["Autenticar", "usuario"])
P3 = proceso(500, 420, "3.0", ["Sincronizar", "empleados"])
P4 = proceso(500, 610, "4.0", ["Gestionar", "inventario"])
P5 = proceso(500, 800, "5.0", ["Registrar", "préstamos"])
P6 = proceso(500, 990, "6.0", ["Registrar", "devoluciones"])
P7 = proceso(500, 1180, "7.0", ["Gestionar", "mantenimiento"])

SX = 930
D1 = almacen(SX, 230, "D1", "Usuarios y roles")
RH = entidad(SX, 395, 240, 64, ["Web service de empleados"])
D2 = almacen(SX, 490, "D2", "Empleados y departamentos", w=290)
D3 = almacen(SX, 610, "D3", "Catálogos")
D4 = almacen(SX, 705, "D4", "Herramientas y piezas", w=270)
D2b = almacen(SX, 800, "D2", "Empleados y departamentos", w=290, dup=True)
D5 = almacen(SX, 895, "D5", "Préstamos")
D4b = almacen(SX, 1085, "D4", "Herramientas y piezas", w=270, dup=True)
D6 = almacen(SX, 1180, "D6", "Mantenimientos")
D5b = almacen(SX, 1275, "D5", "Préstamos", dup=True)
D3b = almacen(SX, 1370, "D3", "Catálogos", dup=True)

P2 = proceso(1400, 230, "2.0", ["Administrar", "usuarios"])
P8 = proceso(1400, 895, "8.0", ["Generar panel", "y reportes"])
AD2 = entidad(1760, 230, 190, 64, ["Administrador"], dup=True)
OP2 = entidad(1760, 760, 190, 80, ["Operador", "(encargado de bodega)"], dup=True)
GE = entidad(1760, 1040, 190, 64, ["Gerencia"])

# ---------------- flujos
# 1.0 Autenticar
flujo(AD, P1, ["Credenciales,", "nueva contraseña"], off=-10, lside=-1)
flujo(P1, AD, ["Resultado del", "inicio de sesión"], off=-10, lpos=0.72)
flujo(OPat(P1), P1, ["Credenciales,", "nueva contraseña"], off=10, lside=1, lpos=0.55)
flujo(P1, OPat(P1), ["Resultado del", "inicio de sesión"], off=10, lside=1, lpos=0.45)
flujo(D1, P1, ["Usuario, rol y hash"], off=10, lside=1)
flujo(P1, D1, ["Nueva contraseña (hash)"], off=10, lside=1)
# 2.0 Administrar usuarios
flujo(AD2, P2, ["Usuarios y roles, restablecer,", "activar / desactivar"], off=10, lside=1)
flujo(P2, AD2, ["Lista de usuarios,", "contraseña temporal"], off=10, lside=1)
flujo(P2, D1, ["Usuario actualizado"], off=0, lside=-1)
flujo(D2, P2, ["Empleado a ligar"], off=0, lside=-1, lpos=0.55)
# 3.0 Sincronizar empleados
flujo(OPat(P3), P3, ["Orden de", "sincronizar"], off=12, lside=-1)
flujo(P3, OPat(P3), ["Lista de empleados,", "resumen de sincronización"], off=12, lside=-1)
flujo(P3, RH, ["Solicitud de sincronización"], off=10, lside=1)
flujo(RH, P3, ["Lista de empleados"], off=10, lside=1)
flujo(P3, D2, ["Empleados nuevos", "y actualizados"], off=0, lside=1, lpos=0.62)
# 4.0 Inventario
flujo(OPat(P4), P4, ["Herramientas, piezas,", "catálogos y bajas"], off=12, lside=-1)
flujo(P4, OPat(P4), ["Inventario y", "disponibilidad"], off=12, lside=-1)
flujo(P4, D3, ["Categorías, marcas,", "ubicaciones, proveedores"], off=0, lside=-1)
flujo(P4, D4, ["Herramienta o pieza", "nueva / dada de baja"], off=0, lside=-1)
# 5.0 Préstamos
flujo(OPat(P5), P5, ["Préstamo: empleado,", "piezas y fecha"], off=12, lside=-1)
flujo(P5, OPat(P5), ["Confirmación", "de préstamo"], off=12, lside=-1)
flujo(D2b, P5, ["Empleado solicitante"], off=0, lside=1)
flujo(D4, P5, ["Piezas disponibles"], off=10, lpos=0.6)
flujo(P5, D4, ["Pieza: Prestada"], off=10, lside=-1, lpos=0.8, force=True)
flujo(P5, D5, ["Préstamo registrado"], off=0, lside=1)
# 6.0 Devoluciones
flujo(OPat(P6), P6, ["Devolución:", "condición y nota"], off=12, lside=-1)
flujo(P6, OPat(P6), ["Confirmación", "de devolución"], off=12, lside=-1)
flujo(P6, D4b, ["Pieza: Disponible,", "Dañada o Perdida"], off=0, lside=-1)
flujo(D5, P6, ["Piezas pendientes"], off=10)
flujo(P6, D5, ["Devolución registrada"], off=10)
# 7.0 Mantenimiento
flujo(OPat(P7), P7, ["Mantenimiento: tipo,", "resultado, costo en L."], off=12, lside=-1)
flujo(P7, OPat(P7), ["Historial de", "mantenimiento"], off=12, lside=-1)
flujo(D5b, P7, ["Quién la devolvió dañada"], off=0, lside=-1, lpos=0.4)
flujo(P7, D6, ["Mantenimiento registrado"], off=10, lside=-1)
flujo(D6, P7, ["Historial"], off=10, lside=-1)
flujo(D3b, P7, ["Proveedor"], off=0, lside=-1, lpos=0.3)
flujo(P7, D4b, ["Pieza: En mantenimiento,", "Disponible o Baja"], off=0, lside=-1, lpos=0.6)
# 8.0 Panel y reportes
flujo(OP2, P8, ["Criterios", "de reportes"], off=12, lside=-1)
flujo(P8, OP2, ["Panel, alertas de vencidos,", "reportes en pantalla"], off=12, lside=-1)
flujo(P8, GE, ["Reportes en", "PDF y Excel"], off=0, lside=1)
flujo(D2, P8, ["Empleados"], off=0, lside=1, lpos=0.55)
flujo(D4, P8, ["Inventario"], off=0, lside=1, lpos=0.4)
flujo(D5, P8, ["Préstamos"], off=0, lside=-1)
flujo(D6, P8, ["Costos de", "mantenimiento"], off=0, lside=1, lpos=0.6)

svg = (f'<svg xmlns="http://www.w3.org/2000/svg" width="{W}" height="{H}" viewBox="0 0 {W} {H}">'
       f'<defs><marker id="f" viewBox="0 0 10 10" refX="9" refY="5" markerWidth="8" markerHeight="8" orient="auto">'
       f'<path d="M0,0 L10,5 L0,10 z" fill="{LINEA}"/></marker></defs>'
       f'<rect width="{W}" height="{H}" fill="#fff"/>' + "".join(out + lin + lab) + '</svg>')
# Las etiquetas se dibujan en orden; las formas deben quedar debajo de las flechas: reordenar
open(sys.argv[1], "w", encoding="utf-8").write(svg)
