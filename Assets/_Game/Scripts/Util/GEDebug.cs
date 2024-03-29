using UnityEngine;
using Unity.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameEngine.Util
{
	/// <summary>
	/// Debug helpers
	/// </summary>

	public static class GEDebug
	{
		/// whether or not debug logs (GEDebug.DebugLogTime, GEDebug.DebugOnScreen) should be displayed
		public static bool DebugLogsEnabled = true;
		/// whether or not debug draws should be executed
		public static bool DebugDrawEnabled = true;

		/// <summary>
		/// Draws a debug ray in 2D and does the actual raycast
		/// </summary>
		/// <returns>The raycast hit.</returns>
		/// <param name="rayOriginPoint">Ray origin point.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="rayDistance">Ray distance.</param>
		/// <param name="mask">Mask.</param>
		/// <param name="debug">If set to <c>true</c> debug.</param>
		/// <param name="color">Color.</param>
		public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && DebugDrawEnabled)
			{
				Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			}
			return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
		}

		public static void DrawPlane(Vector3 position, Vector3 normal)
		{
			Vector3 v3;

			if (normal.normalized != Vector3.forward)
				v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
			else
				v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

			var corner0 = position + v3;
			var corner2 = position - v3;
			var q = Quaternion.AngleAxis(90.0f, normal);
			v3 = q * v3;
			var corner1 = position + v3;
			var corner3 = position - v3;

			Debug.DrawLine(corner0, corner2, Color.green);
			Debug.DrawLine(corner1, corner3, Color.green);
			Debug.DrawLine(corner0, corner1, Color.green);
			Debug.DrawLine(corner1, corner2, Color.green);
			Debug.DrawLine(corner2, corner3, Color.green);
			Debug.DrawLine(corner3, corner0, Color.green);
			Debug.DrawRay(position, normal, Color.red);
		}

		/// <summary>
		/// Does a boxcast and draws a box gizmo
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="size"></param>
		/// <param name="angle"></param>
		/// <param name="direction"></param>
		/// <param name="length"></param>
		/// <param name="mask"></param>
		/// <param name="color"></param>
		/// <param name="drawGizmo"></param>
		/// <returns></returns>
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float length, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && DebugDrawEnabled)
			{
				Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

				Vector3[] points = new Vector3[8];

				float halfSizeX = size.x / 2f;
				float halfSizeY = size.y / 2f;

				points[0] = rotation * (origin + (Vector2.left * halfSizeX) + (Vector2.up * halfSizeY)); // top left
				points[1] = rotation * (origin + (Vector2.right * halfSizeX) + (Vector2.up * halfSizeY)); // top right
				points[2] = rotation * (origin + (Vector2.right * halfSizeX) - (Vector2.up * halfSizeY)); // bottom right
				points[3] = rotation * (origin + (Vector2.left * halfSizeX) - (Vector2.up * halfSizeY)); // bottom left

				points[4] = rotation * ((origin + Vector2.left * halfSizeX + Vector2.up * halfSizeY) + length * direction); // top left
				points[5] = rotation * ((origin + Vector2.right * halfSizeX + Vector2.up * halfSizeY) + length * direction); // top right
				points[6] = rotation * ((origin + Vector2.right * halfSizeX - Vector2.up * halfSizeY) + length * direction); // bottom right
				points[7] = rotation * ((origin + Vector2.left * halfSizeX - Vector2.up * halfSizeY) + length * direction); // bottom left

				Debug.DrawLine(points[0], points[1], color);
				Debug.DrawLine(points[1], points[2], color);
				Debug.DrawLine(points[2], points[3], color);
				Debug.DrawLine(points[3], points[0], color);

				Debug.DrawLine(points[4], points[5], color);
				Debug.DrawLine(points[5], points[6], color);
				Debug.DrawLine(points[6], points[7], color);
				Debug.DrawLine(points[7], points[4], color);

				Debug.DrawLine(points[0], points[4], color);
				Debug.DrawLine(points[1], points[5], color);
				Debug.DrawLine(points[2], points[6], color);
				Debug.DrawLine(points[3], points[7], color);

			}
			return Physics2D.BoxCast(origin, size, angle, direction, length, mask);
		}

		/// <summary>
		/// Draws a debug ray without allocating memory
		/// </summary>
		/// <returns>The ray cast non alloc.</returns>
		/// <param name="array">Array.</param>
		/// <param name="rayOriginPoint">Ray origin point.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="rayDistance">Ray distance.</param>
		/// <param name="mask">Mask.</param>
		/// <param name="color">Color.</param>
		/// <param name="drawGizmo">If set to <c>true</c> draw gizmo.</param>
		public static RaycastHit2D MonoRayCastNonAlloc(RaycastHit2D[] array, Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && DebugDrawEnabled)
			{
				Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			}
			if (Physics2D.RaycastNonAlloc(rayOriginPoint, rayDirection, array, rayDistance, mask) > 0)
			{
				return array[0];
			}
			return new RaycastHit2D();
		}

		/// <summary>
		/// Draws a debug ray in 3D and does the actual raycast
		/// </summary>
		/// <returns>The raycast hit.</returns>
		/// <param name="rayOriginPoint">Ray origin point.</param>
		/// <param name="rayDirection">Ray direction.</param>
		/// <param name="rayDistance">Ray distance.</param>
		/// <param name="mask">Mask.</param>
		/// <param name="debug">If set to <c>true</c> debug.</param>
		/// <param name="color">Color.</param>
		/// <param name="drawGizmo">If set to <c>true</c> draw gizmo.</param>
		public static RaycastHit Raycast3D(Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && DebugDrawEnabled)
			{
				Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			}
			RaycastHit hit;
			Physics.Raycast(rayOriginPoint, rayDirection, out hit, rayDistance, mask);
			return hit;
		}

		/// <summary>
		/// Draws a gizmo arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoArrow(Vector3 origin, Vector3 direction, Color color, float arrowHeadLength = 3f, float arrowHeadAngle = 25f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Gizmos.color = color;
			Gizmos.DrawRay(origin, direction);

			DrawArrowEnd(true, origin, direction, color, arrowHeadLength, arrowHeadAngle);
		}

		/// <summary>
		/// Draws a debug arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color, float arrowHeadLength = 0.2f, float arrowHeadAngle = 35f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Debug.DrawRay(origin, direction, color);

			DrawArrowEnd(false, origin, direction, color, arrowHeadLength, arrowHeadAngle);
		}

		/// <summary>
		/// Draws a debug arrow going from the origin position and along the direction Vector3
		/// </summary>
		/// <param name="origin">Origin.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		/// <param name="arrowLength">Arrow length.</param>
		/// <param name="arrowHeadLength">Arrow head length.</param>
		/// <param name="arrowHeadAngle">Arrow head angle.</param>
		public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color, float arrowLength, float arrowHeadLength = 0.20f, float arrowHeadAngle = 35.0f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Debug.DrawRay(origin, direction * arrowLength, color);

			DrawArrowEnd(false, origin, direction * arrowLength, color, arrowHeadLength, arrowHeadAngle);
		}

		/// <summary>
		/// Draws a debug cross of the specified size and color at the specified point
		/// </summary>
		/// <param name="spot">Spot.</param>
		/// <param name="crossSize">Cross size.</param>
		/// <param name="color">Color.</param>
		public static void DebugDrawCross(Vector3 spot, float crossSize, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 tempOrigin = Vector3.zero;
			Vector3 tempDirection = Vector3.zero;

			tempOrigin.x = spot.x - crossSize / 2;
			tempOrigin.y = spot.y - crossSize / 2;
			tempOrigin.z = spot.z;
			tempDirection.x = 1;
			tempDirection.y = 1;
			tempDirection.z = 0;
			Debug.DrawRay(tempOrigin, tempDirection * crossSize, color);

			tempOrigin.x = spot.x - crossSize / 2;
			tempOrigin.y = spot.y + crossSize / 2;
			tempOrigin.z = spot.z;
			tempDirection.x = 1;
			tempDirection.y = -1;
			tempDirection.z = 0;
			Debug.DrawRay(tempOrigin, tempDirection * crossSize, color);
		}

		/// <summary>
		/// Draws the arrow end for DebugDrawArrow
		/// </summary>
		/// <param name="drawGizmos">If set to <c>true</c> draw gizmos.</param>
		/// <param name="arrowEndPosition">Arrow end position.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="color">Color.</param>
		/// <param name="arrowHeadLength">Arrow head length.</param>
		/// <param name="arrowHeadAngle">Arrow head angle.</param>
		private static void DrawArrowEnd(bool drawGizmos, Vector3 arrowEndPosition, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 40.0f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			if (direction == Vector3.zero)
			{
				return;
			}
			Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
			Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
			Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
			Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
			if (drawGizmos)
			{
				Gizmos.color = color;
				Gizmos.DrawRay(arrowEndPosition + direction, right * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, left * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, up * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, down * arrowHeadLength);
			}
			else
			{
				Debug.DrawRay(arrowEndPosition + direction, right * arrowHeadLength, color);
				Debug.DrawRay(arrowEndPosition + direction, left * arrowHeadLength, color);
				Debug.DrawRay(arrowEndPosition + direction, up * arrowHeadLength, color);
				Debug.DrawRay(arrowEndPosition + direction, down * arrowHeadLength, color);
			}
		}

		/// <summary>
		/// Draws handles to materialize the bounds of an object on screen.
		/// </summary>
		/// <param name="bounds">Bounds.</param>
		/// <param name="color">Color.</param>
		public static void DrawHandlesBounds(Bounds bounds, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

#if UNITY_EDITOR
			Vector3 boundsCenter = bounds.center;
			Vector3 boundsExtents = bounds.extents;

			Vector3 v3FrontTopLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front top left corner
			Vector3 v3FrontTopRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front top right corner
			Vector3 v3FrontBottomLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front bottom left corner
			Vector3 v3FrontBottomRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z - boundsExtents.z);  // Front bottom right corner
			Vector3 v3BackTopLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back top left corner
			Vector3 v3BackTopRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y + boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back top right corner
			Vector3 v3BackBottomLeft = new Vector3(boundsCenter.x - boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back bottom left corner
			Vector3 v3BackBottomRight = new Vector3(boundsCenter.x + boundsExtents.x, boundsCenter.y - boundsExtents.y, boundsCenter.z + boundsExtents.z);  // Back bottom right corner


			Handles.color = color;

			Handles.DrawLine(v3FrontTopLeft, v3FrontTopRight);
			Handles.DrawLine(v3FrontTopRight, v3FrontBottomRight);
			Handles.DrawLine(v3FrontBottomRight, v3FrontBottomLeft);
			Handles.DrawLine(v3FrontBottomLeft, v3FrontTopLeft);

			Handles.DrawLine(v3BackTopLeft, v3BackTopRight);
			Handles.DrawLine(v3BackTopRight, v3BackBottomRight);
			Handles.DrawLine(v3BackBottomRight, v3BackBottomLeft);
			Handles.DrawLine(v3BackBottomLeft, v3BackTopLeft);

			Handles.DrawLine(v3FrontTopLeft, v3BackTopLeft);
			Handles.DrawLine(v3FrontTopRight, v3BackTopRight);
			Handles.DrawLine(v3FrontBottomRight, v3BackBottomRight);
			Handles.DrawLine(v3FrontBottomLeft, v3BackBottomLeft);
#endif
		}

		/// <summary>
		/// Draws a solid rectangle at the specified position and size, and of the specified colors
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="borderColor"></param>
		/// <param name="solidColor"></param>
		public static void DrawSolidRectangle(Vector3 position, Vector3 size, Color borderColor, Color solidColor)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

#if UNITY_EDITOR

			Vector3 halfSize = size / 2f;

			Vector3[] verts = new Vector3[4];
			verts[0] = new Vector3(halfSize.x, halfSize.y, halfSize.z);
			verts[1] = new Vector3(-halfSize.x, halfSize.y, halfSize.z);
			verts[2] = new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
			verts[3] = new Vector3(halfSize.x, -halfSize.y, halfSize.z);
			Handles.DrawSolidRectangleWithOutline(verts, solidColor, borderColor);

#endif
		}

		/// <summary>
		/// Draws a gizmo sphere of the specified size and color at a position
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoPoint(Vector3 position, float size, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}
			Gizmos.color = color;
			Gizmos.DrawWireSphere(position, size);
		}

		/// <summary>
		/// Draws a cube at the specified position, and of the specified color and size
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="color">Color.</param>
		/// <param name="size">Size.</param>
		public static void DrawCube(Vector3 position, Color color, Vector3 size)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 halfSize = size / 2f;

			Vector3[] points = new Vector3[]
			{
				position + new Vector3(halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,-halfSize.y,halfSize.z),
				position + new Vector3(halfSize.x,-halfSize.y,halfSize.z),
				position + new Vector3(halfSize.x,halfSize.y,-halfSize.z),
				position + new Vector3(-halfSize.x,halfSize.y,-halfSize.z),
				position + new Vector3(-halfSize.x,-halfSize.y,-halfSize.z),
				position + new Vector3(halfSize.x,-halfSize.y,-halfSize.z),
			};

			Debug.DrawLine(points[0], points[1], color);
			Debug.DrawLine(points[1], points[2], color);
			Debug.DrawLine(points[2], points[3], color);
			Debug.DrawLine(points[3], points[0], color);
		}

		public static void DrawBounds(Bounds bounds, float delay = 0)
		{
			// bottom
			var p1 = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
			var p2 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
			var p3 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
			var p4 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);

			Debug.DrawLine(p1, p2, Color.blue, delay);
			Debug.DrawLine(p2, p3, Color.red, delay);
			Debug.DrawLine(p3, p4, Color.yellow, delay);
			Debug.DrawLine(p4, p1, Color.magenta, delay);

			// top
			var p5 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
			var p6 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
			var p7 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
			var p8 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

			Debug.DrawLine(p5, p6, Color.blue, delay);
			Debug.DrawLine(p6, p7, Color.red, delay);
			Debug.DrawLine(p7, p8, Color.yellow, delay);
			Debug.DrawLine(p8, p5, Color.magenta, delay);

			// sides
			Debug.DrawLine(p1, p5, Color.white, delay);
			Debug.DrawLine(p2, p6, Color.gray, delay);
			Debug.DrawLine(p3, p7, Color.green, delay);
			Debug.DrawLine(p4, p8, Color.cyan, delay);
		}

		/// <summary>
		/// Draws a cube at the specified position, offset, and of the specified size
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="offset"></param>
		/// <param name="cubeSize"></param>
		/// <param name="wireOnly"></param>
		public static void DrawGizmoCube(Transform transform, Vector3 offset, Vector3 cubeSize, bool wireOnly)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Matrix4x4 rotationMatrix = transform.localToWorldMatrix;
			Gizmos.matrix = rotationMatrix;
			if (wireOnly)
			{
				Gizmos.DrawWireCube(offset, cubeSize);
			}
			else
			{
				Gizmos.DrawCube(offset, cubeSize);
			}
		}

		/// <summary>
		/// Draws a gizmo rectangle
		/// </summary>
		/// <param name="center">Center.</param>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Gizmos.color = color;

			Vector3 v3TopLeft = new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0);
			Vector3 v3TopRight = new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0); ;
			Vector3 v3BottomRight = new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0); ;
			Vector3 v3BottomLeft = new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0); ;

			Gizmos.DrawLine(v3TopLeft, v3TopRight);
			Gizmos.DrawLine(v3TopRight, v3BottomRight);
			Gizmos.DrawLine(v3BottomRight, v3BottomLeft);
			Gizmos.DrawLine(v3BottomLeft, v3TopLeft);
		}

		/// <summary>
		/// Draws a rectangle based on a Rect and color
		/// </summary>
		/// <param name="rectangle">Rectangle.</param>
		/// <param name="color">Color.</param>
		public static void DrawRectangle(Rect rectangle, Color color)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 pos = new Vector3(rectangle.x + rectangle.width / 2, rectangle.y + rectangle.height / 2, 0.0f);
			Vector3 scale = new Vector3(rectangle.width, rectangle.height, 0.0f);

			GEDebug.DrawRectangle(pos, color, scale);
		}

		/// <summary>
		/// Draws a rectangle of the specified color and size at the specified position
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="color">Color.</param>
		/// <param name="size">Size.</param>
		public static void DrawRectangle(Vector3 position, Color color, Vector3 size)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3 halfSize = size / 2f;

			Vector3[] points = new Vector3[]
			{
				position + new Vector3(halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,halfSize.y,halfSize.z),
				position + new Vector3(-halfSize.x,-halfSize.y,halfSize.z),
				position + new Vector3(halfSize.x,-halfSize.y,halfSize.z),
			};

			Debug.DrawLine(points[0], points[1], color);
			Debug.DrawLine(points[1], points[2], color);
			Debug.DrawLine(points[2], points[3], color);
			Debug.DrawLine(points[3], points[0], color);
		}

		/// <summary>
		/// Draws a point of the specified color and size at the specified position
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="col">Col.</param>
		/// <param name="scale">Scale.</param>
		public static void DrawPoint(Vector3 position, Color color, float size, float duration = 0.1f)
		{
			if (!DebugDrawEnabled)
			{
				return;
			}

			Vector3[] points = new Vector3[]
			{
				position + (Vector3.up * size),
				position - (Vector3.up * size),
				position + (Vector3.right * size),
				position - (Vector3.right * size),
				position + (Vector3.forward * size),
				position - (Vector3.forward * size)
			};
			Debug.DrawLine(points[0], points[1], color, duration);
			Debug.DrawLine(points[2], points[3], color, duration);
			Debug.DrawLine(points[4], points[5], color, duration);
			Debug.DrawLine(points[0], points[2], color, duration);
			Debug.DrawLine(points[0], points[3], color, duration);
			Debug.DrawLine(points[0], points[4], color, duration);
			Debug.DrawLine(points[0], points[5], color, duration);
			Debug.DrawLine(points[1], points[2], color, duration);
			Debug.DrawLine(points[1], points[3], color, duration);
			Debug.DrawLine(points[1], points[4], color, duration);
			Debug.DrawLine(points[1], points[5], color, duration);
			Debug.DrawLine(points[4], points[2], color, duration);
			Debug.DrawLine(points[4], points[3], color, duration);
			Debug.DrawLine(points[5], points[2], color, duration);
			Debug.DrawLine(points[5], points[3], color, duration);
		}

#if UNITY_EDITOR
		public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
		{
			if (_color != default(Color))
				Handles.color = _color;
			Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
			using (new Handles.DrawingScope(angleMatrix))
			{
				var pointOffset = (_height - (_radius * 2)) / 2;

				//draw sideways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
				Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
				Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
				//draw frontways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
				Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
				Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
				//draw center
				Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
				Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);
			}
		}
#endif

		//Draws just the box at where it is currently hitting.
		public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
		{
			origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
			DrawBox(origin, halfExtents, orientation, color);
		}

		//Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
		public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
		{
			direction.Normalize();
			Box bottomBox = new Box(origin, halfExtents, orientation);
			Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

			Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
			Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
			Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
			Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
			Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
			Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
			Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
			Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

			DrawBox(bottomBox, color);
			DrawBox(topBox, color);
		}

		public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
		{
			DrawBox(new Box(origin, halfExtents, orientation), color);
		}
		public static void DrawBox(Box box, Color color)
		{
			Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
			Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
			Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
			Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

			Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
			Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
			Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
			Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

			Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
			Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
			Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
			Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
		}

		public static void DrawGizmoBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
		{
			DrawGizmoBox(new Box(origin, halfExtents, orientation), color);
		}
		public static void DrawGizmoBox(Box box, Color color)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(box.frontTopLeft, box.frontTopRight);
			Gizmos.DrawLine(box.frontTopRight, box.frontBottomRight);
			Gizmos.DrawLine(box.frontBottomRight, box.frontBottomLeft);
			Gizmos.DrawLine(box.frontBottomLeft, box.frontTopLeft);

			Gizmos.DrawLine(box.backTopLeft, box.backTopRight);
			Gizmos.DrawLine(box.backTopRight, box.backBottomRight);
			Gizmos.DrawLine(box.backBottomRight, box.backBottomLeft);
			Gizmos.DrawLine(box.backBottomLeft, box.backTopLeft);

			Gizmos.DrawLine(box.frontTopLeft, box.backTopLeft);
			Gizmos.DrawLine(box.frontTopRight, box.backTopRight);
			Gizmos.DrawLine(box.frontBottomRight, box.backBottomRight);
			Gizmos.DrawLine(box.frontBottomLeft, box.backBottomLeft);
		}

		public static void LogColored1(FixedString128Bytes text, Color color)
		{
			Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), text));
		}

		public static void LogColored(FixedString128Bytes text, Color color)
		{
			Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), text));
		}

		public struct Box
		{
			public Vector3 localFrontTopLeft { get; private set; }
			public Vector3 localFrontTopRight { get; private set; }
			public Vector3 localFrontBottomLeft { get; private set; }
			public Vector3 localFrontBottomRight { get; private set; }
			public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
			public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
			public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
			public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

			public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
			public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
			public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
			public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
			public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
			public Vector3 backTopRight { get { return localBackTopRight + origin; } }
			public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
			public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

			public Vector3 origin { get; private set; }

			public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
			{
				Rotate(orientation);
			}
			public Box(Vector3 origin, Vector3 halfExtents)
			{
				this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
				this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
				this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
				this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

				this.origin = origin;
			}


			public void Rotate(Quaternion orientation)
			{
				localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
				localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
				localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
				localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
			}
		}

		//This should work for all cast types
		static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
		{
			return origin + (direction.normalized * hitInfoDistance);
		}

		static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
		{
			Vector3 direction = point - pivot;
			return pivot + rotation * direction;
		}
	}
}